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
        private IEntityWriteService<Property, string> _propertyWriteService;

        public ImportPropertiesJob(IEntityWriteService<Property, string> propertyWriteService)
        {
            _propertyWriteService = propertyWriteService;
        }

        public async Task Run(ClaimsPrincipal user)
        {

            string path = @"M:\My Documents\GitHub\mkealerts\DataSources\parcelbase_mprop_full\parcelbase_mprop_full.shp";

            var projectionInfo = ProjectionInfo.Open(path.Replace(".shp", ".prj"));

            ShapeDataReader reader = new ShapeDataReader(path);
            var mbr = reader.ShapefileBounds;
            var result = reader.ReadByMBRFilter(mbr);
            var coll = result.GetEnumerator();

            Debug.WriteLine("Started: " + DateTime.Now.ToString());

            int i = 0;
            while (i < 1000000 && coll.MoveNext())
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

                    await _propertyWriteService.Create(user, property);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Errored: " + DateTime.Now.ToString());
                    Debug.WriteLine("Error at " + i.ToString());
                    Debug.WriteLine(ex.Message);
                    //throw new Exception("Error creating property", ex);
                }

                //var ellipsoid = /*Ellipsoid.Clarke1866;*/
                //            CoordinateSystemFactory.CreateFlattenedSphere("Clarke 1866", 20925832.16, 294.97470, LinearUnit.USSurveyFoot);

                //var datum = CoordinateSystemFactory.CreateHorizontalDatum("Clarke 1866", DatumType.HD_Geocentric, ellipsoid, null);
                //var gcs = CoordinateSystemFactory.CreateGeographicCoordinateSystem("Clarke 1866", AngularUnit.Degrees, datum,
                //    PrimeMeridian.Greenwich, new AxisInfo("Lon", AxisOrientationEnum.East),
                //    new AxisInfo("Lat", AxisOrientationEnum.North));
                //var parameters = new List<ProjectionParameter>(5)
                //                 {
                //                     new ProjectionParameter("latitude_of_origin", 27.833333333),
                //                     new ProjectionParameter("central_meridian", -99),
                //                     new ProjectionParameter("standard_parallel_1", 28.3833333333),
                //                     new ProjectionParameter("standard_parallel_2", 30.2833333333),
                //                     new ProjectionParameter("false_easting", 2000000/LinearUnit.USSurveyFoot.MetersPerUnit),
                //                     new ProjectionParameter("false_northing", 0)
                //                 };
                //var projection = CoordinateSystemFactory.CreateProjection("Lambert Conic Conformal (2SP)", "lambert_conformal_conic_2sp", parameters);

                //var coordsys = CoordinateSystemFactory.CreateProjectedCoordinateSystem("NAD27 / Texas South Central", gcs, projection, LinearUnit.USSurveyFoot, new AxisInfo("East", AxisOrientationEnum.East), new AxisInfo("North", AxisOrientationEnum.North));

                //var trans = CoordinateTransformationFactory.CreateFromCoordinateSystems(gcs, coordsys);

                //double[] pGeo = new[] { -96, 28.5 };
                //double[] pUtm = trans.MathTransform.Transform(pGeo);
                //double[] pGeo2 = trans.MathTransform.Inverse().Transform(pUtm);

                //double[] expected = new[] { 2963503.91 / LinearUnit.USSurveyFoot.MetersPerUnit, 254759.80 / LinearUnit.USSurveyFoot.MetersPerUnit };
                //Assert.IsTrue(ToleranceLessThan(pUtm, expected, 0.05), TransformationError("LambertConicConformal2SP", expected, pUtm));
                //Assert.IsTrue(ToleranceLessThan(pGeo, pGeo2, 0.0000001), TransformationError("LambertConicConformal2SP", pGeo, pGeo2, true));


                /*
                string lambertConformalConicWkt = @"" +
                    @"PROJCS[""North_America_Lambert_Conformal_Conic""," +
                    @"    GEOGCS[""GCS_North_American_1983""," +
                    @"        DATUM[""North_American_Datum_1983""," +
                    @"            SPHEROID[""GRS_1980"",6378137,298.257222101]]," +
                    @"        PRIMEM[""Greenwich"",0]," +
                    @"        UNIT[""Degree"",0.017453292519943295]]," +
                    @"    PROJECTION[""Lambert_Conformal_Conic_2SP""]," +
                    @"    PARAMETER[""False_Easting"",0]," +
                    @"    PARAMETER[""False_Northing"",0]," +
                    @"    PARAMETER[""Central_Meridian"",-96]," +
                    @"    PARAMETER[""Standard_Parallel_1"",20]," +
                    @"    PARAMETER[""Standard_Parallel_2"",60]," +
                    @"    PARAMETER[""Latitude_Of_Origin"",40]," +
                    @"    UNIT[""Meter"",1]," +
                    @"    AUTHORITY[""EPSG"",""102009""]]";

                string gcsNorthAmerican1927wkt = @"GEOGCS[""GCS_North_American_1927"",DATUM[""D_North_American_1927"",SPHEROID[""Clarke_1866"",6378206.4,294.9786982]],PRIMEM[""Greenwich"",0.0],UNIT[""Degree"",0.0174532925199433]]";

                string webMercatorWkt = @"" +
                    @"PROJCS[""WGS 84 / Pseudo-Mercator""," +
                    @"    GEOGCS[""WGS 84""," +
                    @"        DATUM[""WGS_1984""," +
                    @"            SPHEROID[""WGS 84"",6378137,298.257223563," +
                    @"                AUTHORITY[""EPSG"",""7030""]]," +
                    @"            AUTHORITY[""EPSG"",""6326""]]," +
                    @"        PRIMEM[""Greenwich"",0," +
                    @"            AUTHORITY[""EPSG"",""8901""]]," +
                    @"        UNIT[""degree"",0.0174532925199433," +
                    @"            AUTHORITY[""EPSG"",""9122""]]," +
                    @"            AUTHORITY[""EPSG"",""9122""]]," +
                    @"        AUTHORITY[""EPSG"",""4326""]]," +
                    @"    PROJECTION[""Mercator_1SP""]," +
                    @"    PARAMETER[""central_meridian"",0]," +
                    @"    PARAMETER[""scale_factor"",1]," +
                    @"    PARAMETER[""false_easting"",0]," +
                    @"    PARAMETER[""false_northing"",0]," +
                    @"    UNIT[""metre"",1," +
                    @"        AUTHORITY[""EPSG"",""9001""]]," +
                    @"    AXIS[""X"",EAST]," +
                    @"    AXIS[""Y"",NORTH]," +
                    @"    EXTENSION[""PROJ4"",""+proj=merc +a=6378137 +b=6378137 +lat_ts=0.0 +lon_0=0.0 +x_0=0.0 +y_0=0 +k=1.0 +units=m +nadgrids=@null +wktext  +no_defs""]," +
                    @"    AUTHORITY[""EPSG"",""3857""]]";
                */


                /*
                CoordinateSystemFactory csFact = new CoordinateSystemFactory();
                CoordinateTransformationFactory ctFact = new CoordinateTransformationFactory();

                //ICoordinateSystem sourceSystem = csFact.CreateFromWkt(lambertConformalConicWkt);
                ICoordinateSystem sourceSystem = csFact.CreateFromWkt(gcsNorthAmerican1927wkt);
                //ICoordinateSystem targetSystem = csFact.CreateFromWkt(webMercatorWkt);
                IProjectedCoordinateSystem targetSystem = ProjectedCoordinateSystem.WGS84_UTM(33, true);

                ICoordinateTransformation trans = ctFact.CreateFromCoordinateSystems(sourceSystem, targetSystem);

                Coordinate[] tpoints = trans.MathTransform.TransformList(coll.Current.Geometry.Coordinates).ToArray();
                */

                /*
                CoordinateSystemFactory csFact = new CoordinateSystemFactory();
                CoordinateTransformationFactory ctFact = new CoordinateTransformationFactory();

                ICoordinateSystem utm35ETRS = csFact.CreateFromWkt(
                        "PROJCS[\"ETRS89 / ETRS-TM35\",GEOGCS[\"ETRS89\",DATUM[\"D_ETRS_1989\",SPHEROID[\"GRS_1980\",6378137,298.257222101]],PRIMEM[\"Greenwich\",0],UNIT[\"Degree\",0.017453292519943295]],PROJECTION[\"Transverse_Mercator\"],PARAMETER[\"latitude_of_origin\",0],PARAMETER[\"central_meridian\",27],PARAMETER[\"scale_factor\",0.9996],PARAMETER[\"false_easting\",500000],PARAMETER[\"false_northing\",0],UNIT[\"Meter\",1]]");

                IProjectedCoordinateSystem utm33 = ProjectedCoordinateSystem.WGS84_UTM(33, true);

                ICoordinateTransformation trans = ctFact.CreateFromCoordinateSystems(utm35ETRS, utm33);

                //Coordinate[] points = new Coordinate[]
                //{
                //    new Coordinate(290586.087, 6714000), new Coordinate(290586.392, 6713996.224),
                //    new Coordinate(290590.133, 6713973.772), new Coordinate(290594.111, 6713957.416),
                //    new Coordinate(290596.615, 6713943.567), new Coordinate(290596.701, 6713939.485)
                //};

                Coordinate[] tpoints = trans.MathTransform.TransformList(coll.Current.Geometry.Coordinates).ToArray();
                //for (int i = 0; i < points.Length; i++)
                //    Assert.That(tpoints[i].Equals(trans.MathTransform.Transform(points[i])));
                */


                /*
                GeoAPI.Geometries.Coordinate PinPnt = new GeoAPI.Geometries.Coordinate();
                 NetTopologySuite.IO.WKBReader reader = new NetTopologySuite.IO.WKBReader();
                 var wkb = (byte[])Row["the_geom"];
                 Geometry geom = (Geometry)reader.Read(wkb);
                 var p = new GeometryFeatureProvider(geom);
                 myLayer.DataSource = p;
                 myLayer.Style.Fill = new System.Drawing.SolidBrush(fillcolor);
                ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory ctFact = new ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory();
                myLayer.CoordinateTransformation = ctFact.CreateFromCoordinateSystems(ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84, ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WebMercator);
                myLayer.ReverseCoordinateTransformation = ctFact.CreateFromCoordinateSystems(ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WebMercator, ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84);
                _map.Layers.Add(myLayer);
                */

                /*
                CoordinateTransformationFactory coordinateTransformationFactory = new ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory();
                var coordinateTransformation = coordinateTransformationFactory.CreateFromCoordinateSystems(ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84, ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WebMercator);
                var reverseCoordinateTransformation = coordinateTransformationFactory.CreateFromCoordinateSystems(ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WebMercator, ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84);

                var cf = new ProjNet.CoordinateSystems.CoordinateSystemFactory();

                const string wkt4326 = "GEOGCS[\"WGS 84\",DATUM[\"WGS_1984\",SPHEROID[\"WGS 84\",6378137,298.257223563,AUTHORITY[\"EPSG\",\"7030\"]],AUTHORITY[\"EPSG\",\"6326\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.01745329251994328,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4326\"]]";
                const string wkt3857 = "PROJCS[\"Popular Visualisation CRS / Mercator\", GEOGCS[\"Popular Visualisation CRS\", DATUM[\"Popular Visualisation Datum\", SPHEROID[\"Popular Visualisation Sphere\", 6378137, 0, AUTHORITY[\"EPSG\",\"7059\"]], TOWGS84[0, 0, 0, 0, 0, 0, 0], AUTHORITY[\"EPSG\",\"6055\"] ], PRIMEM[\"Greenwich\", 0, AUTHORITY[\"EPSG\", \"8901\"]], UNIT[\"degree\", 0.0174532925199433, AUTHORITY[\"EPSG\", \"9102\"]], AXIS[\"E\", EAST], AXIS[\"N\", NORTH], AUTHORITY[\"EPSG\",\"4055\"] ], PROJECTION[\"Mercator\"], PARAMETER[\"False_Easting\", 0], PARAMETER[\"False_Northing\", 0], PARAMETER[\"Central_Meridian\", 0], PARAMETER[\"Latitude_of_origin\", 0], UNIT[\"metre\", 1, AUTHORITY[\"EPSG\", \"9001\"]], AXIS[\"East\", EAST], AXIS[\"North\", NORTH], AUTHORITY[\"EPSG\",\"3785\"]]";
                const string wkt3395 = "PROJCS[\"WGS 84 / World Mercator\",GEOGCS[\"WGS 84\",DATUM[\"WGS_1984\",SPHEROID[\"WGS 84\",6378137,298.257223563,AUTHORITY[\"EPSG\",\"7030\"]],AUTHORITY[\"EPSG\",\"6326\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.01745329251994328,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4326\"]],UNIT[\"metre\",1,AUTHORITY[\"EPSG\",\"9001\"]],PROJECTION[\"Mercator_1SP\"],PARAMETER[\"central_meridian\",0],PARAMETER[\"scale_factor\",1],PARAMETER[\"false_easting\",0],PARAMETER[\"false_northing\",0],AUTHORITY[\"EPSG\",\"3395\"],AXIS[\"Easting\",EAST],AXIS[\"Northing\",NORTH]]";

                var sys4326 = cf.CreateFromWkt(wkt4326);
                var sys3857 = cf.CreateFromWkt(wkt3857);
                var sys3395 = cf.CreateFromWkt(wkt3395);
                var transformTo3875 = coordinateTransformationFactory.CreateFromCoordinateSystems(sys4326, sys3857);
                var transformTo3395 = coordinateTransformationFactory.CreateFromCoordinateSystems(sys4326, sys3395);

                */

                //Property property = new Property()
                //{
                //    Id = Guid.NewGuid(),
                //    TAXKEY = coll.Current.Attributes["TAXKEY"].ToString(),
                //    HOUSE_NR_LO = int.Parse(coll.Current.Attributes["HOUSE_NR_L"].ToString()),
                //    HOUSE_NR_HI = int.Parse(coll.Current.Attributes["HOUSE_NR_H"].ToString()),
                //    HOUSE_NR_SFX = coll.Current.Attributes["HOUSE_NR_S"].ToString(),
                //    DIR = coll.Current.Attributes["SDIR"].ToString(),
                //    STREET = coll.Current.Attributes["STREET"].ToString(),
                //    STTYPE = coll.Current.Attributes["STTYPE"].ToString()
                //};

                //await _propertyWriteService.Create(user, property);

                /*
                //if (coll.Current.Attributes["HOUSE_NR_L"].ToString() == "2100" && coll.Current.Attributes["STREET"].ToString() == "WEBSTER")
                if (i % 10000 == 0)
                {
                    //Console.WriteLine(coll.Current);

                    ////ProjectionInfo source = KnownCoordinateSystems.Geographic.NorthAmerica.NAD1927CGQ77; //new ProjectionInfo("+proj=tmerc +lat_0=0 +lon_0=18 +k=0.9999 +x_0=6500000 +y_0=0 +ellps=bessel +towgs84=550.499,164.116,475.142,5.80967,2.07902,-11.62386,0.99999445824 +units=m");
                    //ProjectionInfo source = projectionInfo;
                    ////ProjectionInfo target = KnownCoordinateSystems.Geographic.World.WGS1984;
                    ////ProjectionInfo target = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone33N;
                    //ProjectionInfo target = KnownCoordinateSystems.Geographic.World.ITRF2000;

                    //double[] xy = new double[2]
                    //{
                    //    coll.Current.Geometry.Centroid.X,
                    //    coll.Current.Geometry.Centroid.Y
                    //};
                    //double[] z = new double[1] { 1 };
                    //Reproject.ReprojectPoints(xy, z, source, target, 0, 1);

                    //Console.WriteLine("Latitude: " + xy[0].ToString());
                    //Console.WriteLine("Longitude: " + xy[1].ToString());

                    foreach (var c in coll.Current.Geometry.Coordinates)
                    {
                        Tuple<double, double> coordinates = ReprojectCoordinates(projectionInfo, c.X, c.Y);
                        Debug.WriteLine(coordinates.Item2.ToString() + "," + coordinates.Item1.ToString());
                    }

                    Debug.WriteLine("Done");

                }
                */

                ++i;
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
