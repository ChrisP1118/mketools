using DotSpatial.Projections;
using GeoAPI.CoordinateSystems;
using GeoAPI.CoordinateSystems.Transformations;
using GeoAPI.Geometries;
using MkeAlerts.Web.Models.Data.Properties;
using MkeAlerts.Web.Services;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using NetTopologySuite.IO.ShapeFile.Extended;
using NetTopologySuite.IO.ShapeFile.Extended.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MkeAlerts.Web.Jobs
{
    public class ImportPropertiesJob
    {
        private readonly IEntityWriteService<Property, string> _propertyWriteService;

        public ImportPropertiesJob(IEntityWriteService<Property, string> propertyWriteService)
        {
            _propertyWriteService = propertyWriteService;
        }

        public async Task Run(ClaimsPrincipal user)
        {
            string path = @"M:\My Documents\GitHub\mkealerts\DataSources\parcelbase_mprop_full\parcelbase_mprop_full.shp";

            var projectionInfo = ProjectionInfo.Open(path.Replace(".shp", ".prj"));

            using (ShapeDataReader reader = new ShapeDataReader(path))
            {
                var mbr = reader.ShapefileBounds;
                var result = reader.ReadByMBRFilter(mbr);
                var coll = result.GetEnumerator();

                Debug.WriteLine("Started: " + DateTime.Now.ToString());

                List<Property> properties = new List<Property>();

                int i = 0;
                while (coll.MoveNext())
                {
                    try
                    {
                        Property property = new Property();
                        property.TAXKEY = coll.Current.Attributes["TAXKEY"].ToString();

                        // There are a handful of records without a TAXKEY -- we'll just treat those as invalid, since they may have other weird data
                        if (string.IsNullOrEmpty(property.TAXKEY))
                            continue;

                        property.AIR_CONDIT = coll.Current.Attributes["AIR_CONDIT"].ToString();
                        property.ANGLE = float.Parse(coll.Current.Attributes["ANGLE"].ToString());
                        property.ATTIC = coll.Current.Attributes["ATTIC"].ToString();
                        property.BASEMENT = coll.Current.Attributes["BASEMENT"].ToString();
                        property.BATHS = int.Parse(coll.Current.Attributes["BATHS"].ToString());
                        property.BEDROOMS = int.Parse(coll.Current.Attributes["BEDROOMS"].ToString());
                        property.BLDG_AREA = int.Parse(coll.Current.Attributes["BLDG_AREA"].ToString());
                        property.BLDG_TYPE = coll.Current.Attributes["BLDG_TYPE"].ToString();
                        property.C_A_CLASS = coll.Current.Attributes["C_A_CLASS"].ToString();
                        property.C_A_EXM_IM = int.Parse(coll.Current.Attributes["C_A_EXM_IM"].ToString());
                        property.C_A_EXM_LA = int.Parse(coll.Current.Attributes["C_A_EXM_LA"].ToString());
                        property.C_A_EXM_TO = int.Parse(coll.Current.Attributes["C_A_EXM_TO"].ToString());
                        property.C_A_EXM_TY = coll.Current.Attributes["C_A_EXM_TY"].ToString();
                        property.C_A_IMPRV = int.Parse(coll.Current.Attributes["C_A_IMPRV"].ToString());
                        property.C_A_LAND = int.Parse(coll.Current.Attributes["C_A_LAND"].ToString());
                        property.C_A_SYMBOL = coll.Current.Attributes["C_A_SYMBOL"].ToString();
                        property.C_A_TOTAL = int.Parse(coll.Current.Attributes["C_A_TOTAL"].ToString());
                        property.CHG_NR = coll.Current.Attributes["CHG_NR"].ToString();
                        property.CHK_DIGIT = coll.Current.Attributes["CHK_DIGIT"].ToString();
                        property.CONVEY_DAT = coll.Current.Attributes["CONVEY_DAT"] != null ? (DateTime?)DateTime.Parse(coll.Current.Attributes["CONVEY_DAT"].ToString()) : null;
                        property.CONVEY_FEE = float.Parse(coll.Current.Attributes["CONVEY_FEE"].ToString());
                        property.CONVEY_TYP = coll.Current.Attributes["CONVEY_TYP"].ToString();
                        property.CORNER_LOT = coll.Current.Attributes["CORNER_LOT"].ToString();
                        property.SDIR = coll.Current.Attributes["SDIR"].ToString();
                        property.DIV_DROP = int.Parse(coll.Current.Attributes["DIV_DROP"].ToString());
                        property.DIV_ORG = int.Parse(coll.Current.Attributes["DIV_ORG"].ToString());
                        property.DPW_SANITA = coll.Current.Attributes["DPW_SANITA"].ToString();
                        property.EXM_ACREAG = float.Parse(coll.Current.Attributes["EXM_ACREAG"].ToString());
                        property.EXM_PER__1 = float.Parse(coll.Current.Attributes["EXM_PER__1"].ToString());
                        property.EXM_PER_CT = float.Parse(coll.Current.Attributes["EXM_PER_CT"].ToString());
                        property.FIREPLACE = coll.Current.Attributes["FIREPLACE"].ToString();
                        property.GEO_ALDER = coll.Current.Attributes["GEO_ALDER"].ToString();
                        property.GEO_ALDER_ = coll.Current.Attributes["GEO_ALDER_"].ToString();
                        property.GEO_BI_MAI = coll.Current.Attributes["GEO_BI_MAI"].ToString();
                        property.GEO_BLOCK = coll.Current.Attributes["GEO_BLOCK"].ToString();
                        property.GEO_FIRE = coll.Current.Attributes["GEO_FIRE"].ToString();
                        property.GEO_POLICE = coll.Current.Attributes["GEO_POLICE"].ToString();
                        property.GEO_TRACT = coll.Current.Attributes["GEO_TRACT"].ToString();
                        property.GEO_ZIP_CO = coll.Current.Attributes["GEO_ZIP_CO"].ToString();
                        property.HIST_CODE = coll.Current.Attributes["HIST_CODE"].ToString();
                        property.HOUSE_NR_H = int.Parse(coll.Current.Attributes["HOUSE_NR_H"].ToString());
                        property.HOUSE_NR_L = int.Parse(coll.Current.Attributes["HOUSE_NR_L"].ToString());
                        property.HOUSE_NR_S = coll.Current.Attributes["HOUSE_NR_S"].ToString();
                        property.LAND_USE = coll.Current.Attributes["LAND_USE"].ToString();
                        property.LAND_USE_G = coll.Current.Attributes["LAND_USE_G"].ToString();
                        property.LAST_NAME_ = coll.Current.Attributes["LAST_NAME_"] != null ? (DateTime?)DateTime.Parse(coll.Current.Attributes["LAST_NAME_"].ToString()) : null;
                        property.LAST_VALUE = coll.Current.Attributes["LAST_VALUE"] != null ? (DateTime?)DateTime.Parse(coll.Current.Attributes["LAST_VALUE"].ToString()) : null;
                        property.LOT_AREA = int.Parse(coll.Current.Attributes["LOT_AREA"].ToString());
                        property.NEIGHBORHO = coll.Current.Attributes["NEIGHBORHO"].ToString();
                        property.NR_ROOMS = coll.Current.Attributes["NR_ROOMS"].ToString();
                        property.NR_STORIES = float.Parse(coll.Current.Attributes["NR_STORIES"].ToString());
                        property.NR_UNITS = int.Parse(coll.Current.Attributes["NR_UNITS"].ToString());
                        property.OWNER_CITY = coll.Current.Attributes["OWNER_CITY"].ToString();
                        property.OWNER_MAIL = coll.Current.Attributes["OWNER_MAIL"].ToString();
                        property.OWNER_NA_1 = coll.Current.Attributes["OWNER_NA_1"].ToString();
                        property.OWNER_NA_2 = coll.Current.Attributes["OWNER_NA_2"].ToString();
                        property.OWNER_NAME = coll.Current.Attributes["OWNER_NAME"].ToString();
                        property.OWNER_ZIP = coll.Current.Attributes["OWNER_ZIP"].ToString();
                        property.OWN_OCPD = coll.Current.Attributes["OWN_OCPD"].ToString();
                        property.P_A_CLASS = coll.Current.Attributes["P_A_CLASS"].ToString();
                        property.P_A_EXM_IM = int.Parse(coll.Current.Attributes["P_A_EXM_IM"].ToString());
                        property.P_A_EXM_LA = int.Parse(coll.Current.Attributes["P_A_EXM_LA"].ToString());
                        property.P_A_EXM_TO = int.Parse(coll.Current.Attributes["P_A_EXM_TO"].ToString());
                        property.P_A_EXM_TY = coll.Current.Attributes["P_A_EXM_TY"].ToString();
                        property.P_A_IMPRV = int.Parse(coll.Current.Attributes["P_A_IMPRV"].ToString());
                        property.P_A_LAND = int.Parse(coll.Current.Attributes["P_A_LAND"].ToString());
                        property.P_A_SYMBOL = coll.Current.Attributes["P_A_SYMBOL"].ToString();
                        property.P_A_TOTAL = int.Parse(coll.Current.Attributes["P_A_TOTAL"].ToString());
                        property.PARCEL_TYP = float.Parse(coll.Current.Attributes["PARCEL_TYP"].ToString());
                        property.PARKING_SP = float.Parse(coll.Current.Attributes["PARKING_SP"].ToString());
                        property.PARKING_TY = coll.Current.Attributes["PARKING_TY"].ToString();
                        property.PLAT_PAGE = coll.Current.Attributes["PLAT_PAGE"].ToString();
                        property.POWDER_ROO = int.Parse(coll.Current.Attributes["POWDER_ROO"].ToString());
                        property.RAZE_STATU = coll.Current.Attributes["RAZE_STATU"].ToString();
                        property.REASON_FOR = coll.Current.Attributes["REASON_FOR"].ToString();
                        property.STREET = coll.Current.Attributes["STREET"].ToString();
                        property.STTYPE = coll.Current.Attributes["STTYPE"].ToString();
                        property.SUB_ACCT = coll.Current.Attributes["SUB_ACCT"].ToString();
                        property.SWIM_POOL = coll.Current.Attributes["SWIM_POOL"].ToString();
                        property.TAX_RATE_C = coll.Current.Attributes["TAX_RATE_C"].ToString();
                        property.BI_VIOL = coll.Current.Attributes["BI_VIOL"].ToString();
                        property.TAX_DELQ = int.Parse(coll.Current.Attributes["TAX_DELQ"].ToString());
                        property.YR_ASSMT = coll.Current.Attributes["YR_ASSMT"].ToString();
                        property.YR_BUILT = coll.Current.Attributes["YR_BUILT"].ToString();
                        property.ZONING = coll.Current.Attributes["ZONING"].ToString();

                        IGeometryFactory geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

                        IPoint projectedCentroid = ReprojectCoordinates(projectionInfo, coll.Current.Geometry.Centroid);
                        IPoint transformedCentroid = geometryFactory.CreatePoint(new Coordinate(projectedCentroid.X, projectedCentroid.Y));
                        property.Centroid = transformedCentroid;

                        Coordinate[] projectedCoordinates = ReprojectCoordinates(projectionInfo, coll.Current.Geometry.Coordinates);
                        Polygon transformedGeometry = (Polygon)geometryFactory.CreatePolygon(projectedCoordinates);

                        // https://gis.stackexchange.com/questions/289545/using-sqlgeometry-makevalid-to-get-a-counter-clockwise-polygon-in-sql-server
                        if (!transformedGeometry.Shell.IsCCW)
                            transformedGeometry = (Polygon)transformedGeometry.Reverse();

                        property.Parcel = transformedGeometry;

                        properties.Add(property);

                        if (i % 100 == 0)
                        {
                            await _propertyWriteService.Create(user, properties);
                            properties = new List<Property>();
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Errored: " + DateTime.Now.ToString());
                        Debug.WriteLine("Error at " + i.ToString());
                        Debug.WriteLine(ex.Message);
                        //throw new Exception("Error creating property", ex);
                    }

                    ++i;
                }

                await _propertyWriteService.Create(user, properties);
            }

            Debug.WriteLine("Finished: " + DateTime.Now.ToString());
        }

        public Tuple<double, double> ReprojectCoordinates(ProjectionInfo source, double x, double y)
        {
            ProjectionInfo target = KnownCoordinateSystems.Geographic.World.ITRF2000;

            // This is what Google, Bing, etc. use for mapping - but projecting to it seems to end up with coordinates that are off by a few hundred feet
            //ProjectionInfo target = KnownCoordinateSystems.Geographic.World.WGS1984;

            double[] xy = new double[2]
            {
                x,
                y
            };
            double[] z = new double[1] { 1 };
            Reproject.ReprojectPoints(xy, z, source, target, 0, 1);

            return new Tuple<double, double>(xy[0], xy[1]);
        }

        public IPoint ReprojectCoordinates(ProjectionInfo source, IPoint point)
        {
            Tuple<double, double> coordinates = ReprojectCoordinates(source, point.X, point.Y);
            return new Point(coordinates.Item1, coordinates.Item2);
        }

        public Coordinate[] ReprojectCoordinates(ProjectionInfo source, Coordinate[] coordinates)
        {
            Coordinate[] retVal = new Coordinate[coordinates.Length];
            for (int i = 0; i < coordinates.Length; ++i)
            {
                Tuple<double, double> coordinateSet = ReprojectCoordinates(source, coordinates[i].X, coordinates[i].Y);
                retVal[i] = new Coordinate(coordinateSet.Item1, coordinateSet.Item2);
            }
            return retVal;
        }

    }
}
