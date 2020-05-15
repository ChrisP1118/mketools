using CsvHelper;
using CsvHelper.Configuration;
using DotSpatial.Projections;
using Hangfire;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MkeAlerts.Web.Models.Data;
using MkeAlerts.Web.Models.Data.Accounts;
using MkeAlerts.Web.Models.Data.Places;
using MkeAlerts.Web.Services;
using MkeAlerts.Web.Services.Data;
using MkeAlerts.Web.Services.Data.Interfaces;
using MkeAlerts.Web.Services.Functional;
using MkeAlerts.Web.Utilities;
using NetTopologySuite;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using NetTopologySuite.IO.ShapeFile.Extended;
using NetTopologySuite.IO.ShapeFile.Extended.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Parcel = MkeAlerts.Web.Models.Data.Places.Parcel;

namespace MkeAlerts.Web.Jobs
{
    public class ImportPropertiesArchivesJob : LoggedJob
    {
        private readonly IEntityWriteService<Property, Guid> _writeService;
		private readonly IParcelService _parcelService;

        public ImportPropertiesArchivesJob(IConfiguration configuration, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IMailerService mailerService, IJobRunService jobRunService, ILogger<ImportPropertiesArchivesJob> logger, IEntityWriteService<Property, Guid> writeService, IParcelService parcelService)
            : base(configuration, signInManager, userManager, mailerService, jobRunService, logger)
        {
            _writeService = writeService;
			_parcelService = parcelService;
        }

        protected override async Task RunInternal()
        {
			List<MpropField> mpropFields = GetMpropFields();

			ClaimsPrincipal claimsPrincipal = await GetClaimsPrincipal();

			_logger.LogInformation("Getting parcel TAXKEYs");

			HashSet<string> taxkeys = await _parcelService.GetAllTaxkeys(claimsPrincipal);

			_logger.LogInformation("Loaded {TaxkeyCount} TAXKEYs", taxkeys.Count);

			List<string> files = Directory.GetFiles(_configuration.GetValue<string>("MpropArchiveFolder"), "*.csv").OrderByDescending(x => GetFileYearFromFile(GetFileNameShort(x))).ToList();
			//List<string> files = Directory.GetFiles(@"M:\My Documents\GitHub\mpropsandbox\Data", "mprop2019dec.csv").OrderByDescending(x => GetFileYearFromFile(GetFileNameShort(x))).ToList();
			List<Property> items = new List<Property>();

			int i = 0;

			foreach (string file in files)
			{
				string fileNameShort = GetFileNameShort(file).ToLower();
				int row = 0;
				DateTime sourceDate = new DateTime(GetFileYearFromFile(fileNameShort), 12, 31);

				_logger.LogInformation("Reading {FileNameShort}", fileNameShort);

				using (var streamReader = new StreamReader(file))
				using (var csvReader = new CsvReader(streamReader, new CsvConfiguration(CultureInfo.InvariantCulture)
				{
					BadDataFound = (ReadingContext context) => { },
					MissingFieldFound = (string[] headerNames, int index, ReadingContext context) => { }
				}))
				{
					csvReader.Read();
					csvReader.ReadHeader();

					while (csvReader.Read())
					{
						++i;
						++row;

						try
						{
							Property property = new Property();
							property.Id = Guid.NewGuid();
							property.Source = fileNameShort;
							property.SourceDate = sourceDate;

							foreach (MpropField mpropField in mpropFields)
							{
								string rawValue = null;

								foreach (string dataFileName in mpropField.DataFileNames)
								{
									rawValue = csvReader.GetField(dataFileName);
									if (rawValue != null)
										break;
								}

								if (rawValue == null)
									continue;

								rawValue = rawValue.Trim();

								if (mpropField.PropertyInfo.PropertyType == typeof(string))
								{
									MaxLengthAttribute maxLengthAttribute = mpropField.PropertyInfo.GetCustomAttribute<MaxLengthAttribute>();
									if (maxLengthAttribute != null && rawValue.Length > maxLengthAttribute.Length)
										rawValue = rawValue.Substring(0, maxLengthAttribute.Length);

									if (rawValue.Length > 0)
										mpropField.PropertyInfo.SetValue(property, rawValue);
								}
								else if (mpropField.PropertyInfo.PropertyType == typeof(int?))
								{
									int n = 0;
									if (int.TryParse(rawValue, out n))
										mpropField.PropertyInfo.SetValue(property, n);
								}
								else if (mpropField.PropertyInfo.PropertyType == typeof(float?))
								{
									float n = 0;
									if (float.TryParse(rawValue, out n))
										mpropField.PropertyInfo.SetValue(property, n);
								}
								else if (mpropField.PropertyInfo.PropertyType == typeof(DateTime?))
								{
									// Leading zeros are missing - maybe they were lost in Excel when converting to CSV?
									if (rawValue.Length == 3)
										rawValue = "0" + rawValue;

									if (rawValue.Length == 5)
										rawValue = "0" + rawValue;

									try
									{
										if (rawValue.Length == 4)
										{
											// I'll regret this in 2030
											if (int.TryParse(rawValue.Substring(0, 2), out int y) && int.TryParse(rawValue.Substring(2, 2), out int m))
												if (m != 0)
													mpropField.PropertyInfo.SetValue(property, new DateTime(y < 30 ? 2000 + y : 1900 + y, m, 1));
										}
										else if (rawValue.Length == 6)
										{
											if (int.TryParse(rawValue.Substring(0, 2), out int m) && int.TryParse(rawValue.Substring(2, 2), out int d) && int.TryParse(rawValue.Substring(4, 2), out int y))
												if (m > 12)
													// Some of the older files use a different date format, YYMMDD
													mpropField.PropertyInfo.SetValue(property, new DateTime(m, d, y));
												else if (m != 0 && d != 0)
													mpropField.PropertyInfo.SetValue(property, new DateTime(y < 30 ? 2000 + y : 1900 + y, m, d));
										}
										else if (rawValue.Length > 6)
										{
											if (DateTime.TryParse(rawValue, out DateTime dt))
												mpropField.PropertyInfo.SetValue(property, new DateTime?(dt));
										}
									}
									catch (Exception ex)
									{
										_logger.LogError("Error parsing DateTime value {Value} on row {Row} in {FileNameShort}", rawValue, row, fileNameShort);
									}
								}
								else
								{
									throw new Exception("Unhandled type");
								}
							}

							// Don't add items if the TAXKEY doesn't exist in parcels -- it'll just create a foreign key violation that will slow down the bulk insert
							if (taxkeys.Contains(property.TAXKEY))
							{
								while (property.TAXKEY.Length < 10)
									property.TAXKEY = "0" + property.TAXKEY;

								items.Add(property);
							}
						}
						catch (Exception ex)
						{
							_logger.LogError("Error inserting row {Row} in {FileNameShort}", row, fileNameShort);
						}

						if (i % 100 == 0)
						{
							try
							{
								Tuple<IEnumerable<Property>, IEnumerable<Property>> results1 = await _writeService.BulkUpsert(claimsPrincipal, items, true);
								_successCount += results1.Item1.Count();
								_failureCount += results1.Item2.Count();

								_logger.LogDebug("Bulk inserted items at mod {Index}", i);
							}
							catch (Exception ex)
							{
								_failureCount += items.Count;

								_logger.LogError(ex, "Error bulk inserting items at mod {Index}", i);
							}
							items.Clear();
						}
					}
				}
			}
		}

		protected string GetFileNameShort(string file)
		{
			string fileNameShort = file.Substring(file.LastIndexOf("\\") + 1);
			return fileNameShort.Replace(".csv", "");
		}

		protected int GetFileYearFromFile(string fileNameShort)
		{
			string fileYearShort = fileNameShort;

			if (fileYearShort.ToLower().StartsWith("mprop") && fileYearShort.Length == 7)
				fileYearShort = fileYearShort.Substring(5);
			else if (fileYearShort.ToLower().StartsWith("mprop") && fileYearShort.Length == 12)
				fileYearShort = fileYearShort.Substring(7, 2);

			int fileYear = int.Parse(fileYearShort);
			if (fileYear < 30)
				fileYear += 2000;
			else
				fileYear += 1900;

			return fileYear;
		}

		/// <summary>
		/// This list comes from the MpropSandbox project (https://github.com/ChrisP1118/mpropsandbox)
		/// </summary>
		/// <returns></returns>
		protected List<MpropField> GetMpropFields()
        {
			List<MpropField> mpropFields = new List<MpropField>()
			{
				new MpropField
				{
					DataModelName = "AIR_CONDITIONING",
					PropertyInfo = typeof(Property).GetProperty("AIR_CONDITIONING"),
					DataFileNames = new List<string>()
					{
						"AIR_CONDITIONING",
						"AIRCONDIT",
						"AIR_CONDIT",
						"AIRCOND",
					}
				},
				new MpropField
				{
					DataModelName = "ATTIC",
					PropertyInfo = typeof(Property).GetProperty("ATTIC"),
					DataFileNames = new List<string>()
					{
						"ATTIC",
					}
				},
				new MpropField
				{
					DataModelName = "BASEMENT",
					PropertyInfo = typeof(Property).GetProperty("BASEMENT"),
					DataFileNames = new List<string>()
					{
						"BASEMENT",
					}
				},
				new MpropField
				{
					DataModelName = "BATHS",
					PropertyInfo = typeof(Property).GetProperty("BATHS"),
					DataFileNames = new List<string>()
					{
						"BATHS",
						"BATHROOM",
					}
				},
				new MpropField
				{
					DataModelName = "BEDROOMS",
					PropertyInfo = typeof(Property).GetProperty("BEDROOMS"),
					DataFileNames = new List<string>()
					{
						"BEDROOMS",
					}
				},
				new MpropField
				{
					DataModelName = "BLDG_AREA",
					PropertyInfo = typeof(Property).GetProperty("BLDG_AREA"),
					DataFileNames = new List<string>()
					{
						"BLDG_AREA",
						"BLDGAREA",
					}
				},
				new MpropField
				{
					DataModelName = "BLDG_TYPE",
					PropertyInfo = typeof(Property).GetProperty("BLDG_TYPE"),
					DataFileNames = new List<string>()
					{
						"BLDG_TYPE",
						"BLDGTYPE",
						"BUILDTYP",
					}
				},
				new MpropField
				{
					DataModelName = "C_A_CLASS",
					PropertyInfo = typeof(Property).GetProperty("C_A_CLASS"),
					DataFileNames = new List<string>()
					{
						"C_A_CLASS",
						"CACLASS",
						"CURCLSCD",
					}
				},
				new MpropField
				{
					DataModelName = "C_A_EXM_IMPRV",
					PropertyInfo = typeof(Property).GetProperty("C_A_EXM_IMPRV"),
					DataFileNames = new List<string>()
					{
						"C_A_EXM_IMPRV",
						"CAEXMIMPRV",
						"C_A_EXM_IM",
						"CUREXIMP",
					}
				},
				new MpropField
				{
					DataModelName = "C_A_EXM_LAND",
					PropertyInfo = typeof(Property).GetProperty("C_A_EXM_LAND"),
					DataFileNames = new List<string>()
					{
						"C_A_EXM_LAND",
						"CAEXMLAND",
						"C_A_EXM_LA",
						"CUREXLND",
					}
				},
				new MpropField
				{
					DataModelName = "C_A_EXM_TOTAL",
					PropertyInfo = typeof(Property).GetProperty("C_A_EXM_TOTAL"),
					DataFileNames = new List<string>()
					{
						"C_A_EXM_TOTAL",
						"CAEXMTOTAL",
						"C_A_EXM_TO",
						"CUREXTOT",
					}
				},
				new MpropField
				{
					DataModelName = "C_A_EXM_TYPE",
					PropertyInfo = typeof(Property).GetProperty("C_A_EXM_TYPE"),
					DataFileNames = new List<string>()
					{
						"C_A_EXM_TYPE",
						"CAEXMTYPE",
						"C_A_EXM_TY",
						"CUREXTYP",
					}
				},
				new MpropField
				{
					DataModelName = "C_A_IMPRV",
					PropertyInfo = typeof(Property).GetProperty("C_A_IMPRV"),
					DataFileNames = new List<string>()
					{
						"C_A_IMPRV",
						"CAIMPRV",
						"CURIMPAS",
					}
				},
				new MpropField
				{
					DataModelName = "C_A_LAND",
					PropertyInfo = typeof(Property).GetProperty("C_A_LAND"),
					DataFileNames = new List<string>()
					{
						"C_A_LAND",
						"CALAND",
						"CURLNDAS",
					}
				},
				new MpropField
				{
					DataModelName = "C_A_SYMBOL",
					PropertyInfo = typeof(Property).GetProperty("C_A_SYMBOL"),
					DataFileNames = new List<string>()
					{
						"C_A_SYMBOL",
						"CASYMBOL",
						"CURSYMBL",
					}
				},
				new MpropField
				{
					DataModelName = "C_A_TOTAL",
					PropertyInfo = typeof(Property).GetProperty("C_A_TOTAL"),
					DataFileNames = new List<string>()
					{
						"C_A_TOTAL",
						"CATOTAL",
						"CURTOTAS",
					}
				},
				new MpropField
				{
					DataModelName = "CHK_DIGIT",
					PropertyInfo = typeof(Property).GetProperty("CHK_DIGIT"),
					DataFileNames = new List<string>()
					{
						"CHK_DIGIT",
						"CHKDIGIT",
						"CHECKDIG",
					}
				},
				new MpropField
				{
					DataModelName = "CONVEY_DATE",
					PropertyInfo = typeof(Property).GetProperty("CONVEY_DATE"),
					DataFileNames = new List<string>()
					{
						"CONVEY_DATE",
						"CONVEYDATE",
						"CONVEY_DAT",
						"CONVDATE",
					}
				},
				new MpropField
				{
					DataModelName = "CONVEY_FEE",
					PropertyInfo = typeof(Property).GetProperty("CONVEY_FEE"),
					DataFileNames = new List<string>()
					{
						"CONVEY_FEE",
						"CONVEYFEE",
						"CONVFEE",
					}
				},
				new MpropField
				{
					DataModelName = "CONVEY_TYPE",
					PropertyInfo = typeof(Property).GetProperty("CONVEY_TYPE"),
					DataFileNames = new List<string>()
					{
						"CONVEY_TYPE",
						"CONVEYTYPE",
						"CONVEY_TYP",
						"CONVTYPE",
					}
				},
				new MpropField
				{
					DataModelName = "DIV_ORG",
					PropertyInfo = typeof(Property).GetProperty("DIV_ORG"),
					DataFileNames = new List<string>()
					{
						"DIV_ORG",
						"DIVORG",
					}
				},
				new MpropField
				{
					DataModelName = "FIREPLACE",
					PropertyInfo = typeof(Property).GetProperty("FIREPLACE"),
					DataFileNames = new List<string>()
					{
						"FIREPLACE",
						"FIREPLAC",
					}
				},
				new MpropField
				{
					DataModelName = "GARAGE_TYPE",
					PropertyInfo = typeof(Property).GetProperty("GARAGE_TYPE"),
					DataFileNames = new List<string>()
					{
						"GARAGE_TYPE",
						"GARAGETYPE",
						"GARAGE",
					}
				},
				new MpropField
				{
					DataModelName = "GEO_ALDER",
					PropertyInfo = typeof(Property).GetProperty("GEO_ALDER"),
					DataFileNames = new List<string>()
					{
						"GEO_ALDER",
						"GEOALDER",
						"ALDERDIS",
					}
				},
				new MpropField
				{
					DataModelName = "GEO_ALDER_OLD",
					PropertyInfo = typeof(Property).GetProperty("GEO_ALDER_OLD"),
					DataFileNames = new List<string>()
					{
						"GEO_ALDER_OLD",
						"GEOALDERO",
						"GEO_ALDER_",
						"ALDEROLD",
					}
				},
				new MpropField
				{
					DataModelName = "GEO_BLOCK",
					PropertyInfo = typeof(Property).GetProperty("GEO_BLOCK"),
					DataFileNames = new List<string>()
					{
						"GEO_BLOCK",
						"GEOBLOCK",
						"CENBLOCK",
					}
				},
				new MpropField
				{
					DataModelName = "GEO_POLICE",
					PropertyInfo = typeof(Property).GetProperty("GEO_POLICE"),
					DataFileNames = new List<string>()
					{
						"GEO_POLICE",
						"GEOPOLICE",
						"POLICEDS",
					}
				},
				new MpropField
				{
					DataModelName = "GEO_TRACT",
					PropertyInfo = typeof(Property).GetProperty("GEO_TRACT"),
					DataFileNames = new List<string>()
					{
						"GEO_TRACT",
						"GEOTRACT",
						"CENTRACT",
					}
				},
				new MpropField
				{
					DataModelName = "GEO_ZIP_CODE",
					PropertyInfo = typeof(Property).GetProperty("GEO_ZIP_CODE"),
					DataFileNames = new List<string>()
					{
						"GEO_ZIP_CODE",
						"GEOZIPCODE",
						"GEO_ZIP_CO",
					}
				},
				new MpropField
				{
					DataModelName = "HIST_CODE",
					PropertyInfo = typeof(Property).GetProperty("HIST_CODE"),
					DataFileNames = new List<string>()
					{
						"HIST_CODE",
						"HISTCODE",
						"HISTDES",
					}
				},
				new MpropField
				{
					DataModelName = "HOUSE_NR_HI",
					PropertyInfo = typeof(Property).GetProperty("HOUSE_NR_HI"),
					DataFileNames = new List<string>()
					{
						"HOUSE_NR_HI",
						"HOUSENRHI",
						"HOUSE_NR_H",
						"HSNUMHI",
					}
				},
				new MpropField
				{
					DataModelName = "HOUSE_NR_LO",
					PropertyInfo = typeof(Property).GetProperty("HOUSE_NR_LO"),
					DataFileNames = new List<string>()
					{
						"HOUSE_NR_LO",
						"HOUSENRLO",
						"HOUSE_NR_L",
						"HSNUMLO",
					}
				},
				new MpropField
				{
					DataModelName = "HOUSE_NR_SFX",
					PropertyInfo = typeof(Property).GetProperty("HOUSE_NR_SFX"),
					DataFileNames = new List<string>()
					{
						"HOUSE_NR_SFX",
						"HOUSENRSFX",
						"HOUSE_NR_S",
						"HSNUMSUF",
					}
				},
				new MpropField
				{
					DataModelName = "LAND_USE",
					PropertyInfo = typeof(Property).GetProperty("LAND_USE"),
					DataFileNames = new List<string>()
					{
						"LAND_USE",
						"LANDUSE",
					}
				},
				new MpropField
				{
					DataModelName = "LAND_USE_GP",
					PropertyInfo = typeof(Property).GetProperty("LAND_USE_GP"),
					DataFileNames = new List<string>()
					{
						"LAND_USE_GP",
						"LANDUSEGP",
						"LAND_USE_G",
					}
				},
				new MpropField
				{
					DataModelName = "LAST_NAME_CHG",
					PropertyInfo = typeof(Property).GetProperty("LAST_NAME_CHG"),
					DataFileNames = new List<string>()
					{
						"LAST_NAME_CHG",
						"LASTNAMECH",
						"LAST_NAME_",
						"NAMCHDAT",
					}
				},
				new MpropField
				{
					DataModelName = "LAST_VALUE_CHG",
					PropertyInfo = typeof(Property).GetProperty("LAST_VALUE_CHG"),
					DataFileNames = new List<string>()
					{
						"LAST_VALUE_CHG",
						"LASTVALUE",
						"LAST_VALUE",
					}
				},
				new MpropField
				{
					DataModelName = "LOT_AREA",
					PropertyInfo = typeof(Property).GetProperty("LOT_AREA"),
					DataFileNames = new List<string>()
					{
						"LOT_AREA",
						"LOTAREA",
					}
				},
				new MpropField
				{
					DataModelName = "NEIGHBORHOOD",
					PropertyInfo = typeof(Property).GetProperty("NEIGHBORHOOD"),
					DataFileNames = new List<string>()
					{
						"NEIGHBORHOOD",
						"NEIGHBORHD",
						"NEIGHBORHO",
						"NBRHOOD",
					}
				},
				new MpropField
				{
					DataModelName = "NR_STORIES",
					PropertyInfo = typeof(Property).GetProperty("NR_STORIES"),
					DataFileNames = new List<string>()
					{
						"NR_STORIES",
						"NRSTORIES",
						"NSTORIES",
					}
				},
				new MpropField
				{
					DataModelName = "NR_UNITS",
					PropertyInfo = typeof(Property).GetProperty("NR_UNITS"),
					DataFileNames = new List<string>()
					{
						"NR_UNITS",
						"NRUNITS",
						"NUMUNITS",
					}
				},
				new MpropField
				{
					DataModelName = "OWN_OCPD",
					PropertyInfo = typeof(Property).GetProperty("OWN_OCPD"),
					DataFileNames = new List<string>()
					{
						"OWN_OCPD",
						"OWNOCPD",
						"OWNEROCC",
					}
				},
				new MpropField
				{
					DataModelName = "OWNER_CITY_STATE",
					PropertyInfo = typeof(Property).GetProperty("OWNER_CITY_STATE"),
					DataFileNames = new List<string>()
					{
						"OWNER_CITY_STATE",
						"OWNERCITY",
						"OWNER_CITY",
						"OWNRCITY",
					}
				},
				new MpropField
				{
					DataModelName = "OWNER_MAIL_ADDR",
					PropertyInfo = typeof(Property).GetProperty("OWNER_MAIL_ADDR"),
					DataFileNames = new List<string>()
					{
						"OWNER_MAIL_ADDR",
						"OWNERADDR",
						"OWNER_MAIL",
						"OWNRADDR",
					}
				},
				new MpropField
				{
					DataModelName = "OWNER_NAME_1",
					PropertyInfo = typeof(Property).GetProperty("OWNER_NAME_1"),
					DataFileNames = new List<string>()
					{
						"OWNER_NAME_1",
						"OWNERNAME1",
						"OWNRNAM1",
					}
				},
				new MpropField
				{
					DataModelName = "OWNER_NAME_2",
					PropertyInfo = typeof(Property).GetProperty("OWNER_NAME_2"),
					DataFileNames = new List<string>()
					{
						"OWNER_NAME_2",
						"OWNERNAME2",
						"OWNRNAM2",
					}
				},
				new MpropField
				{
					DataModelName = "OWNER_NAME_3",
					PropertyInfo = typeof(Property).GetProperty("OWNER_NAME_3"),
					DataFileNames = new List<string>()
					{
						"OWNER_NAME_3",
						"OWNERNAME3",
						"OWNRNAM3",
					}
				},
				new MpropField
				{
					DataModelName = "OWNER_ZIP",
					PropertyInfo = typeof(Property).GetProperty("OWNER_ZIP"),
					DataFileNames = new List<string>()
					{
						"OWNER_ZIP",
						"OWNERZIP",
						"OWNRZIP",
					}
				},
				new MpropField
				{
					DataModelName = "P_A_CLASS",
					PropertyInfo = typeof(Property).GetProperty("P_A_CLASS"),
					DataFileNames = new List<string>()
					{
						"P_A_CLASS",
						"PACLASS",
						"PRECLSCD",
					}
				},
				new MpropField
				{
					DataModelName = "P_A_EXM_IMPRV",
					PropertyInfo = typeof(Property).GetProperty("P_A_EXM_IMPRV"),
					DataFileNames = new List<string>()
					{
						"P_A_EXM_IMPRV",
						"PAEXMIMPRV",
						"P_A_EXM_IM",
						"PREEXIMP",
					}
				},
				new MpropField
				{
					DataModelName = "P_A_EXM_LAND",
					PropertyInfo = typeof(Property).GetProperty("P_A_EXM_LAND"),
					DataFileNames = new List<string>()
					{
						"P_A_EXM_LAND",
						"PAEXMLAND",
						"P_A_EXM_LA",
						"PREEXLND",
					}
				},
				new MpropField
				{
					DataModelName = "P_A_EXM_TOTAL",
					PropertyInfo = typeof(Property).GetProperty("P_A_EXM_TOTAL"),
					DataFileNames = new List<string>()
					{
						"P_A_EXM_TOTAL",
						"PAEXMTOTAL",
						"P_A_EXM_TO",
						"PREEXTOT",
					}
				},
				new MpropField
				{
					DataModelName = "P_A_IMPRV",
					PropertyInfo = typeof(Property).GetProperty("P_A_IMPRV"),
					DataFileNames = new List<string>()
					{
						"P_A_IMPRV",
						"PAIMPRV",
						"PREIMPAS",
					}
				},
				new MpropField
				{
					DataModelName = "P_A_LAND",
					PropertyInfo = typeof(Property).GetProperty("P_A_LAND"),
					DataFileNames = new List<string>()
					{
						"P_A_LAND",
						"PALAND",
						"PRELNDAS",
					}
				},
				new MpropField
				{
					DataModelName = "P_A_SYMBOL",
					PropertyInfo = typeof(Property).GetProperty("P_A_SYMBOL"),
					DataFileNames = new List<string>()
					{
						"P_A_SYMBOL",
						"PASYMBOL",
						"PRESYMBL",
					}
				},
				new MpropField
				{
					DataModelName = "P_A_TOTAL",
					PropertyInfo = typeof(Property).GetProperty("P_A_TOTAL"),
					DataFileNames = new List<string>()
					{
						"P_A_TOTAL",
						"PATOTAL",
						"PRETOTAS",
					}
				},
				new MpropField
				{
					DataModelName = "PLAT_PAGE",
					PropertyInfo = typeof(Property).GetProperty("PLAT_PAGE"),
					DataFileNames = new List<string>()
					{
						"PLAT_PAGE",
						"PLATPAGE",
					}
				},
				new MpropField
				{
					DataModelName = "POWDER_ROOMS",
					PropertyInfo = typeof(Property).GetProperty("POWDER_ROOMS"),
					DataFileNames = new List<string>()
					{
						"POWDER_ROOMS",
						"POWDERROOM",
						"POWDER_ROO",
						"PWDRROOM",
					}
				},
				new MpropField
				{
					DataModelName = "REASON_FOR_CHG",
					PropertyInfo = typeof(Property).GetProperty("REASON_FOR_CHG"),
					DataFileNames = new List<string>()
					{
						"REASON_FOR_CHG",
						"REASONFOR",
						"REASON_FOR",
						"REASFCHG",
					}
				},
				new MpropField
				{
					DataModelName = "SDIR",
					PropertyInfo = typeof(Property).GetProperty("SDIR"),
					DataFileNames = new List<string>()
					{
						"SDIR",
						"DIR",
						"STRTDIR",
					}
				},
				new MpropField
				{
					DataModelName = "Source",
					PropertyInfo = typeof(Property).GetProperty("Source"),
					DataFileNames = new List<string>()
					{
						"Source",
					}
				},
				new MpropField
				{
					DataModelName = "STREET",
					PropertyInfo = typeof(Property).GetProperty("STREET"),
					DataFileNames = new List<string>()
					{
						"STREET",
						"STRTNAME",
					}
				},
				new MpropField
				{
					DataModelName = "STTYPE",
					PropertyInfo = typeof(Property).GetProperty("STTYPE"),
					DataFileNames = new List<string>()
					{
						"STTYPE",
						"STRTTYPE",
					}
				},
				new MpropField
				{
					DataModelName = "TAX_RATE_CD",
					PropertyInfo = typeof(Property).GetProperty("TAX_RATE_CD"),
					DataFileNames = new List<string>()
					{
						"TAX_RATE_CD",
						"TAXRATECD",
						"TAX_RATE_C",
						"TAXDIST",
						"TAXRATEC",
					}
				},
				new MpropField
				{
					DataModelName = "TAXKEY",
					PropertyInfo = typeof(Property).GetProperty("TAXKEY"),
					DataFileNames = new List<string>()
					{
						"TAXKEY",
					}
				},
				new MpropField
				{
					DataModelName = "YR_ASSMT",
					PropertyInfo = typeof(Property).GetProperty("YR_ASSMT"),
					DataFileNames = new List<string>()
					{
						"YR_ASSMT",
						"YRASSMT",
						"YEARASS",
					}
				},
				new MpropField
				{
					DataModelName = "YR_BUILT",
					PropertyInfo = typeof(Property).GetProperty("YR_BUILT"),
					DataFileNames = new List<string>()
					{
						"YR_BUILT",
						"YRBUILT",
					}
				},
				new MpropField
				{
					DataModelName = "ZONING",
					PropertyInfo = typeof(Property).GetProperty("ZONING"),
					DataFileNames = new List<string>()
					{
						"ZONING",
					}
				},
			};

			return mpropFields;
		}
	}
}
