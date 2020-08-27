using Microsoft.Extensions.Logging;
using MkeTools.Web.Models.Data;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO.ShapeFile.Extended.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MkeTools.Web.Utilities
{
    public static class ShapefileUtilities
    {
        public static bool CopyFields(IShapefileFeature source, object target, int i, ILogger logger)
        {
            foreach (PropertyInfo propertyInfo in target.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (propertyInfo.PropertyType == typeof(int))
                {
                    string name = propertyInfo.Name;
                    if (source.Attributes.Exists(name) && source.Attributes[name] != null)
                    {
                        int val = 0;
                        if (int.TryParse(source.Attributes[name].ToString(), out val))
                            propertyInfo.SetValue(target, val);
                    }
                }
                else if (propertyInfo.PropertyType == typeof(double))
                {
                    string name = propertyInfo.Name;
                    if (source.Attributes.Exists(name) && source.Attributes[name] != null)
                    {
                        double val = 0;
                        if (double.TryParse(source.Attributes[name].ToString(), out val))
                            propertyInfo.SetValue(target, val);
                    }
                }
                else if (propertyInfo.PropertyType == typeof(string))
                {
                    string name = propertyInfo.Name;
                    if (source.Attributes.Exists(name) && source.Attributes[name] != null)
                    {
                        propertyInfo.SetValue(target, source.Attributes[name].ToString());
                    }
                }
                else if (typeof(Geometry).IsAssignableFrom(propertyInfo.PropertyType))
                {
                    Geometry transformedGeometry = null;

                    if (source.Geometry.SRID == 0)
                    {
                        transformedGeometry = source.Geometry;
                        transformedGeometry.SRID = 4326;
                    }
                    else
                    {
                        GeometryFactory geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

                        if (source.Geometry.GeometryType == "Polygon")
                        {
                            transformedGeometry = geometryFactory.CreatePolygon(source.Geometry.Coordinates);
                        }
                        else if (source.Geometry.GeometryType == "MultiPolygon")
                        {
                            List<Polygon> polygons = new List<Polygon>();
                            foreach (Polygon polygon in ((MultiPolygon)source.Geometry).Geometries)
                            {
                                polygons.Add((Polygon)geometryFactory.CreatePolygon(polygon.Coordinates));
                            }
                            transformedGeometry = geometryFactory.CreateMultiPolygon(polygons.ToArray());
                        }
                        else if (source.Geometry.GeometryType == "LineString")
                        {
                            transformedGeometry = geometryFactory.CreateLineString(source.Geometry.Coordinates);
                        }
                        else if (source.Geometry.GeometryType == "Point")
                        {
                            transformedGeometry = geometryFactory.CreatePoint(source.Geometry.Coordinate);
                        }
                    }

                    // https://gis.stackexchange.com/questions/289545/using-sqlgeometry-makevalid-to-get-a-counter-clockwise-polygon-in-sql-server
                    if (transformedGeometry != null && transformedGeometry is Polygon && !((Polygon)transformedGeometry).Shell.IsCCW)
                        transformedGeometry = (Polygon)transformedGeometry.Reverse();
                    if (transformedGeometry != null && transformedGeometry is MultiPolygon)
                    {
                        MultiPolygon multiPolygon = (MultiPolygon)transformedGeometry;
                        Geometry[] geometries = multiPolygon.Geometries.Select(x =>
                            x is Polygon && !((Polygon)x).Shell.IsCCW ? ((Polygon)x).Reverse() : x
                        ).ToArray();

                        for (int j = 0; j < multiPolygon.Geometries.Length; ++j)
                            multiPolygon.Geometries[j] = geometries[j];
                    }

                    if (transformedGeometry == null)
                    {
                        logger.LogTrace("Skipping record " + i.ToString() + " - No transformed geometry");
                        return false;
                    }

                    propertyInfo.SetValue(target, transformedGeometry);

                    if (typeof(IHasBounds).IsAssignableFrom(target.GetType()))
                        GeographicUtilities.SetBounds((IHasBounds)target, transformedGeometry);
                }
            }

            return true;
        }
    }
}
