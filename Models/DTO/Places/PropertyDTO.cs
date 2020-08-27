using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MkeTools.Web.Models.DTO.Places
{
    public class PropertyDTO
    {
        /// <summary>
        /// Unique identifier for this record
        /// </summary>
        public Guid Id { get; set; }

        public ParcelDTO Parcel { get; set; }

        /// <summary>
        /// The data source for the property record.
        /// </summary>
        /// <remarks>
        /// Older MPROP files included just a year (MPROP75), newer ones include a month (MPROP2019DEC).
        /// </remarks>
        [MaxLength(12)]
        public string Source { get; set; }

        /// <summary>
        /// The date that source was created.
        /// </summary>
        /// <remarks>
        /// For historical MPROP records, this'll be 12/31 of the year.
        /// </remarks>
        public DateTime SourceDate { get; set; }

        /// <summary>
        /// A unique ten digit number assigned by the City of Milwaukee, to each property.
        /// </summary>
        [MaxLength(10)]
        public string TAXKEY { get; set; }

        /// <summary>
        /// The year for which the current assessment data refers.
        /// </summary>
        [MaxLength(4)]
        public string YR_ASSMT { get; set; }

        /// <summary>
        /// An additional digit which verifies that the taxkey is correct e.g., a check digit.
        /// </summary>
        [MaxLength(1)]
        public string CHK_DIGIT { get; set; }

        /// <summary>
        /// The code that determines what county tax rate is to be used for this property.
        /// </summary>
        /// <remarks>
        /// * 1 – Milwaukee County
        /// * 2 – Washington County
        /// * 3 – Waukesha County
        /// </remarks>
        [MaxLength(2)]
        public string TAX_RATE_CD { get; set; }

        /// <summary>
        /// Indicates the page number of the Assessor's plat book upon which a map of the property is found.
        /// </summary>
        /// <remarks>
        /// Normally, the first three digits identify the quarter section number and the last two digits identify the page number within that quarter section.
        /// </remarks>
        [MaxLength(5)]
        public string PLAT_PAGE { get; set; }

        /// <summary>
        /// The house number if it is a single family residence.
        /// </summary>
        /// <remarks>
        /// If the property has more than one address (such as a duplex or multiple structure), then this is the number on the low end of the range.
        /// </remarks>
        public int? HOUSE_NR_LO { get; set; }

        /// <summary>
        /// The number on the high end of the range of addresses.
        /// </summary>
        /// <remarks>
        /// If the property has only one number, then this field will contain the same number as HOUSE-NR-LOW.
        /// </remarks>
        public int? HOUSE_NR_HI { get; set; }

        /// <summary>
        /// A three character field used (if necessary) to further identify a house number.
        /// </summary>
        [MaxLength(3)]
        public string HOUSE_NR_SFX { get; set; }

        /// <summary>
        /// This field contains the street direction: "E, N, S, or W".
        /// </summary>
        [MaxLength(1)]
        public string SDIR { get; set; }

        /// <summary>
        /// The standard street name for the property address.
        /// </summary>
        [MaxLength(18)]
        public string STREET { get; set; }

        /// <summary>
        /// The last element of the property address is a two position abbreviation for street type (AV, ST, PL, etc.).
        /// </summary>
        /// <remarks>
        /// A few streets do not have a street type.
        /// </remarks>
        [MaxLength(2)]
        public string STTYPE { get; set; }

        /// <summary>
        /// The current year's assessment class code that identifies the general type of property use.
        /// </summary>
        /// <remarks>
        /// * 1 Residential (RES)
        /// * 2 Mercantile (MER)
        /// * 3 Manufacturing (MFG)
        /// * 4 Special Mercantile (SME)
        /// * 5 Condominiums (CDM)
        /// * 7 Mercantile Apartments (4 or more units) (MAP)
        /// * 9 Exempt (EXM)
        /// </remarks>
        [MaxLength(1)]
        public string C_A_CLASS { get; set; }

        /// <summary>
        /// The code indicating status of assessment:
        /// </summary>
        /// <remarks>
        /// * U - Unfinished
        /// * L - Doomage(assessor was denied entry)
        /// </remarks>
        [MaxLength(1)]
        public string C_A_SYMBOL { get; set; }

        /// <summary>
        /// The current assessed value of the land as determined by the City Assessor.
        /// </summary>
        public int? C_A_LAND { get; set; }

        /// <summary>
        /// The current assessed value of all improvements on the property as determined by the City Assessor.
        /// </summary>
        public int? C_A_IMPRV { get; set; }

        /// <summary>
        /// The sum of current land and improvement assessment.The current total assessment.
        /// </summary>
        public int? C_A_TOTAL { get; set; }

        /// <summary>
        /// Assessor’s Exemption Code.
        /// </summary>
        /// <remarks>
        /// * RELIGIOUS INSTITUTION
        ///   * 10: Traditional (Construction) Church
        ///   * 11: Store-Front Church
        ///   * 12: Store-Front/Liv Units
        ///   * 20: Housing (i.e. Pastors, Ordained Ministers)
        ///   * 30: Religious School
        ///   * 40: Parking
        ///   * 50: Convents or Sisters’ Homes
        ///   * 80: Miscellaneous
        /// * EDUCATIONAL INSTITUTION
        ///   * 100: Educational Institutions
        ///   * 110: Non-Profit Day Care
        ///   * 101: Educational Association
        ///   * 111: Benevolent Association
        ///   * 113: Miscellaneous Educational Institution
        /// * COLLEGES AND UNIVERSITIES
        ///   * 90: Colleges and Universities
        ///   * 91: Housing (Dormitories)
        ///   * 92: Miscellaneous Colleges and Universities
        ///   * 130: CEMETARIES
        ///   * 170: MEMORIALS
        ///   * 171: WASTE TREATMENT FACILITY
        /// * BENEVOLENT INSTITUTIONS
        ///   * 270: Women’s Clubs
        ///   * 271: Historical Societies
        ///   * 160: Fraternal/Veterans Organizations
        ///   * 272: Libraries
        ///   * 180: Community Redevelopment Groups
        ///   * 220: Nursing Homes/Homes For The Aged
        ///   * 221: Retirement Homes For The Aged
        ///   * 222: Assisted Living
        ///   * 223: Transitional Living
        ///   * 250: Group Homes --- including CBRF
        ///   * 210: Children's Homes
        ///   * 230: Y.M.C.A & Y.W.C.A. Community Centers
        ///   * 240: Salvation Army, Goodwill, St. Vincent de Paul
        ///   * 260: Miscellaneous Benevolent
        ///   * 190: Low Income/Disabled Housing
        /// * NON-PROFIT HOSPITALS
        ///   * 200: Non-Profit Hospitals
        ///   * 201: Housing For Student Nurses
        ///   * 202: Miscellaneous Non-Profit Hospitals
        ///   * 280: LABOR TEMPLES
        /// * PROPERTY HELD FOR REHABILITATION
        ///   * 181: Habitat for Humanity
        ///   * 182: Miscellaneous Property Held for Rehabilitation
        /// * 183: INSTITUTIONS FOR DEPENDENT CHILDREN
        /// * 240: BOY/GIRL SCOUTS & BOYS’/GIRLS’ CLUBS OF AMERICA
        /// * 240: SALVATION ARMY
        /// * 245: PROPERTY HELD IN TRUST IN PUBLIC INTEREST
        /// * 202: NON-PROFIT MEDICAL RESEARCH FOUNDATIONS
        /// * 405: SPORTS & ENTERTAINMENT FACILITIES
        /// * 405: PROFESSIONAL SPORTS HOME FACILITIES
        /// * 406: HUMANE SOCIETIES
        /// * 407: NON-PROFIT THEATRES
        /// * UNITED STATES GOVERNMENT
        ///   * 310: General
        ///   * 320: Indian Reservations
        ///   * 321: Miscellaneous United States Government
        /// * PROPERTY OF THE STATE
        ///   * 340: General
        ///   * 360: Highways
        ///   * 330: University of Wisconsin
        ///   * 350: Department of Veterans’ Affairs
        ///   * 351: Miscellaneous Property of the State
        /// * MILWAUKEE COUNTY
        ///   * 390: General
        ///   * 401: Housing
        ///   * 402: Metro Sewer
        ///   * 400: X-Way (Trust for Wisconsin)
        ///   * 410: Airport
        ///   * 420: County Parks
        ///   * 430: Tax Deed & Welfare
        ///   * 440: County Highway
        /// * MUNICIPAL PROPERTY
        ///   * 480: General
        ///   * 500: Housing Authority
        ///   * 540: Land Banks
        ///   * 520: Playground/Tot Lot/Green Spot
        ///   * 750: Public Schools, School Sites incl Voc and all MPS
        ///   * 490: Redevelopment
        ///   * 530: Tax Deed
        ///   * 510: Vacant Land/Parking
        ///   * 570: Wisconsin Center District
        ///   * 571: Miscellaneous Municipal Property
        ///   * 550: Vocational Schools
        ///   * 660: Water Works
        ///   * 720: Fire Department
        /// * RAILROADS
        ///   * 780: Soo Line
        ///   * 790: Northwestern
        ///   * 810: CMC Real Estate
        ///   * 800: Miscellaneous Railroads
        /// * UTILITIES
        ///   * 820: Air Carriers
        ///   * 830: Gas
        ///   * 840: Electric
        ///   * 850: Telephone
        ///   * 860: Pipelines
        ///   * 900: Streets, Alleys, Ped Ways
        /// </remarks>
        [MaxLength(3)]
        public string C_A_EXM_TYPE { get; set; }

        /// <summary>
        /// The current exempt value of the land as determined by the City Assessor.
        /// </summary>
        public int? C_A_EXM_LAND { get; set; }

        /// <summary>
        /// The current exempt value of all improvements on the property as determined by the City Assessor.
        /// </summary>
        public int? C_A_EXM_IMPRV { get; set; }

        /// <summary>
        /// The sum of current exempt land and improvement assessments.The current total assessment exemptions.
        /// </summary>
        public int? C_A_EXM_TOTAL { get; set; }

        /// <summary>
        /// The previous year's assessment class that identifies the general type of property use.
        /// </summary>
        [MaxLength(1)]
        public string P_A_CLASS { get; set; }

        /// <summary>
        /// The code indicating previous assessment year's status of assessment.
        /// </summary>
        [MaxLength(4)]
        public string P_A_SYMBOL { get; set; }

        /// <summary>
        /// The previous year's assessed value of the land as determined by the City Assessor.
        /// </summary>
        public int? P_A_LAND { get; set; }

        /// <summary>
        /// The previous year's assessed value of all improvements on the property as determined by the city assessor.
        /// </summary>
        public int? P_A_IMPRV { get; set; }

        /// <summary>
        /// The sum of the previous year's land and improvement assessment. The previous year's total assessment.
        /// </summary>
        public int? P_A_TOTAL { get; set; }

        /// <summary>
        /// The previous year's exempt value of land as determined by the City Assessor.
        /// </summary>
        public int? P_A_EXM_LAND { get; set; }

        /// <summary>
        /// The previous year's exempt value of all improvements on the property as determined by the city assessor.
        /// </summary>
        public int? P_A_EXM_IMPRV { get; set; }

        /// <summary>
        /// The sum of the previous year's exempt land and exempt improvement. The previous year's total assessment exemptions.
        /// </summary>
        public int? P_A_EXM_TOTAL { get; set; }

        /// <summary>
        /// The last date when current assessments were changed.
        /// </summary>
        public DateTime? LAST_VALUE_CHG { get; set; }

        /// <summary>
        /// The first position contains the same value as C-A-SYMBOL. The remaining 3 positions contain any combination of up to 3 single character code entries of reasons for change of assessment.
        /// </summary>
        /// <remarks>
        /// * A: ADDED ROOMS
        /// * B: BASEMENT REMODELING
        /// * C: LAND CHANGE
        /// * D: DEMOLISHED STRUCTURE
        /// * E: EXTERIOR CHANGES
        /// * G: GARAGE
        /// * I: INTERIOR CHANGES
        /// * N: NEW CONSTRUCTION OR ADDITION
        /// * P: ADDED PLUMBING
        /// * R: REVALUATION OR REVIEW
        /// * S: SIDING
        /// * T: CENTRAL AIR CONDITIONING
        /// </remarks>
        [MaxLength(3)]
        public string REASON_FOR_CHG { get; set; }

        /// <summary>
        /// The year and month of the last real-estate conveyance transaction.
        /// </summary>
        /// <remarks>
        /// The original data stores just a month and year, not a day of the month; so the day of the month used here will always be the 1st.
        /// </remarks>
        public DateTime? CONVEY_DATE { get; set; }

        /// <summary>
        /// The method used to convey the property.
        /// </summary>
        /// <remarks>
        /// * AD: Administrator's Deed
        /// * AF: Abridgement(change) of Final Decree or Affidavit
        /// * AJ: Abridgement(change) of Judgement of Divorce
        /// * AL: Assignment of Land Contract or Lease
        /// * AP: Affidavit of Adverse Possession
        /// * AS: Affidavit of Survivorship
        /// * AW: Awards of Damages (Governmental Agency Condemnation)
        /// * CD: Condominium Deed
        /// * CO: Court Order
        /// * CS: Certificate of Survivorship
        /// * D: Deed
        /// * DV: Divorce Judgement
        /// * DL: Judgement of Descent of Lands
        /// * FD: Final Decree
        /// * FJ: Final Judgement
        /// * GD: Guardian's Deed
        /// * IR: Judgement - In Rem Action
        /// * JD: Judgement of Divorce
        /// * JF: Judgement of Foreclosure (Land Contract Foreclosure)
        /// * LC: Land Contract
        /// * LS: Lease
        /// * MD: Marshall's Deed
        /// * PR: Personal Representative Deed
        /// * QC: Quit Claim
        /// * RD: Receiver's Deed
        /// * RS: Resolution Accepting Reservations for Public Use
        /// * SD: Sheriff's Deed/Certificate of Sale/Affidavit of Sale
        /// * TD: Trustee's Deed
        /// * WD: Warranty Deed
        /// * E: Estate - Termination of interest or trans surv JT
        /// * LD: Land Contract
        /// * PD: Personal Representative Deed
        /// * SH: Sheriff's Deed
        /// * SW: Special Warranty Deed
        /// </remarks>
        [MaxLength(2)]
        public string CONVEY_TYPE { get; set; }

        /// <summary>
        /// The real-estate transfer(recording) fee amount paid at the time of purchase of a property.
        /// </summary>
        /// <remarks>
        /// This is a fee based on the total value of real-estate transferred.
        /// 
        /// The transfer fee paid on real-estate transfers after 8/31/81 are computed as follows:
        /// (the REAL ESTATE value) * .003
        /// The transfer fee due on real-estate transfers before 9/1/81 are computed as follows:
        /// (the REAL ESTATE value) * .001 
        /// </remarks>
        public float? CONVEY_FEE { get; set; }

        /// <summary>
        /// A control number which references internal Assessor's Office files.
        /// </summary>
        /// <remarks>
        /// It relates to internal documents which identify the origin of the taxkey.
        /// </remarks>
        public int? DIV_ORG { get; set; }

        /// <summary>
        /// Name of the legal property owner.
        /// </summary>
        [MaxLength(28)]
        public string OWNER_NAME_1 { get; set; }

        /// <summary>
        /// The continuation of OWNER-NAME-1 or second owner name.
        /// </summary>
        [MaxLength(28)]
        public string OWNER_NAME_2 { get; set; }

        /// <summary>
        /// The continuation of OWNER-NAME-1 or OWNER-NAME-2 or a third owner name.
        /// </summary>
        [MaxLength(28)]
        public string OWNER_NAME_3 { get; set; }

        /// <summary>
        /// The mailing address of the property owner. 
        /// </summary>
        [MaxLength(28)]
        public string OWNER_MAIL_ADDR { get; set; }

        /// <summary>
        /// The city and state of the owner's address.
        /// </summary>
        [MaxLength(28)]
        public string OWNER_CITY_STATE { get; set; }

        /// <summary>
        /// The zip code of the owner's address.
        /// </summary>
        /// <remarks>
        /// The OWNER-ZIP field normally has data only in the first 5 positions.
        /// </remarks>
        [MaxLength(9)]
        public string OWNER_ZIP { get; set; }

        /// <summary>
        /// The date that the last owner name changed or the owner address changed.
        /// </summary>
        public DateTime? LAST_NAME_CHG { get; set; }

        /// <summary>
        /// This is the Assessor's Office neighborhood designator.
        /// </summary>
        /// <remarks>
        /// The City has been divided into approximately 200 different neighborhoods.
        /// </remarks>
        [MaxLength(8)]
        public string NEIGHBORHOOD { get; set; }

        /// <summary>
        /// The building type code.
        /// </summary>
        /// <remarks>
        /// BLDG-TYPE for non-residential properties is no no longer maintained by the Assessor’s Office. All occurrences for non-residential properties will contain spaces.
        /// 
        /// For condominiums the code is defined as follows:
        /// * APARTMENT
        /// * BI/TRI
        /// * BILEVEL
        /// * COTTAGE
        /// * DUPLEX
        /// * GARDEN
        /// * HI-RISE
        /// * LOFTS
        /// * MID-RISE
        /// * MIXED
        /// * PARKING R
        /// * POOL
        /// * RANCH
        /// * ROW HOUSE
        /// * SPLIT LEV
        /// * TOWNHOUSE
        /// * TRIPLEX
        /// 
        /// NOTE: BLDG-TYPE is no longer maintained for non-residential parcels.
        /// 
        /// NOTE: Assessor's Building Type Codes do not always completely or adequately describe the use characteristic of the property. Additional information related to use can be obtained by the review of the LAND-USE sic code assigned to the property in combination with the BLDG- TYPE code assigned. 
        /// </remarks>
        [MaxLength(9)]
        public string BLDG_TYPE { get; set; }

        /// <summary>
        /// The number of stories above grade in the building (does not include the basement.)
        /// </summary>
        /// <remarks>
        /// For multi-structure properties, the number of stories of the predominant building is shown.
        /// </remarks>
        public float? NR_STORIES { get; set; }

        /// <summary>
        /// Basement characteristics.
        /// </summary>
        /// <remarks>
        /// This field contains data for residential properties, i.e, residential building types and commercial apartment and housing buildings.
        /// 
        /// Basement Code:
        /// * F: Full
        /// * N: None
        /// * P: Partial
        /// * C: Crawl Space
        /// </remarks>
        [MaxLength(1)]
        public string BASEMENT { get; set; }

        /// <summary>
        /// Attic characteristics.
        /// </summary>
        /// <remarks>
        /// This field contains data for residential properties only, i.e, residential building types and commercial apartment buildings.
        /// 
        /// Attic Code:
        /// * Y: Yes
        /// * N: No
        /// </remarks>
        [MaxLength(1)]
        public string ATTIC { get; set; }

        /// <summary>
        /// Indicates the number of dwelling units on the property.
        /// </summary>
        /// <remarks>
        /// If there is more than 1 structure, then this field indicates the total number of dwelling units in all structures.
        /// 
        /// For hotels and motels, it is the total number of rooms.
        /// 
        /// For nursing homes, it is the total number of licensed beds.
        /// 
        /// For rooming houses it is the actual number of units for the tax file.
        /// 
        /// This field contains data on properties containing residential and apartment buildings only, i.e, residential building types and commercial apartment buildings.
        /// </remarks>
        public int? NR_UNITS { get; set; }

        /// <summary>
        /// The total useable floor area of the structure in square feet.
        /// </summary>
        public int? BLDG_AREA { get; set; }

        /// <summary>
        /// The year of construction of the structure on the property.
        /// </summary>
        /// <remarks>
        /// For properties with more than one structure, the year built of the most prominent structure is used.This fiel is only maintained for residential parcels
        /// </remarks>
        public int? YR_BUILT { get; set; }

        /// <summary>
        /// A "1" is used to indicate a fireplace.
        /// </summary>
        /// <remarks>
        ///  This field contains data on residential properties only, i.e, residential building types and commercial apartment buildings.
        /// </remarks>
        [MaxLength(1)]
        public string FIREPLACE { get; set; }

        /// <summary>
        /// A "1" in the first position is used to indicate central air conditioning.
        /// </summary>
        /// <remarks>
        /// Positions 2 and 3 should be ignored.This field contains reliable data for residential single and duplex structures only.
        /// </remarks>
        [MaxLength(3)]
        public string AIR_CONDITIONING { get; set; }

        /// <summary>
        /// The total number of bedrooms.
        /// </summary>
        /// <remarks>
        /// If more than one structure is on the property, then this field indicates the total bedrooms of all structures.
        ///  
        /// If this is an apartment building, then this field indicates the total number of bedrooms in the structure.
        /// 
        /// This field contains data on residential properties only, i.e, residential building types and commercial apartment buildings.
        /// </remarks>
        public int? BEDROOMS { get; set; }

        /// <summary>
        /// The total number of bathrooms in the building, or the number of bathrooms predominantly found in each dwelling unit.
        /// </summary>
        /// <remarks>
        /// If more then one building, than this includes the total for all structures.
        /// 
        /// This field contains reliable data on residential single and duplex properties.
        /// 
        /// For apartment buildings this represents the typical number of baths per unit.
        /// </remarks>
        public int? BATHS { get; set; }

        /// <summary>
        /// The number of half-bathrooms or powder rooms in the building, or the number of powder rooms predominantly found in each dwelling unit of a multi-unit structure.
        /// </summary>
        /// <remarks>
        /// This field contains reliable data on residential single and duplex properties.
        /// 
        /// For apartment buildings this represents the typical number of half-baths per unit.
        /// </remarks>
        public int? POWDER_ROOMS { get; set; }

        /// <summary>
        /// Garage characteristics.
        /// </summary>
        /// <remarks>
        ///  This field contains data on residential properties only, i.e, residential building types and commercial apartment buildings.
        ///  
        /// Garage Type:
        /// * "A" for an attached garage
        /// * "D" for a detached garage
        /// * "AD" for both
        /// </remarks>
        [MaxLength(2)]
        public string GARAGE_TYPE { get; set; }

        /// <summary>
        /// The size of the property in square feet. 
        /// </summary>
        public int? LOT_AREA { get; set; }

        /// <summary>
        /// The current zoning of the property as identified on the City Zoning Map.
        /// </summary>
        /// <remarks>
        /// For further clarification refer to the computerized zoning ordinances available through the City of Milwaukee website at www.mkedcd.org/czo or contact the City of Milwaukee, Department of City Development, Land Use Planning Section.
        /// </remarks>
        [MaxLength(7)]
        public string ZONING { get; set; }

        /// <summary>
        /// A four-digit number code based upon the Standard Industrial Classification code(SIC) identifying the type of activity on this property.
        /// </summary>
        [MaxLength(4)]
        public string LAND_USE { get; set; }

        /// <summary>
        /// A value from 0 to 13 showing the general use of land that this property falls within.
        /// </summary>
        [MaxLength(2)]
        public string LAND_USE_GP { get; set; }

        /// <summary>
        /// A code signifying that the property is occupied by the owner.
        /// </summary>
        /// <remarks>
        /// This is determined by comparing the Property Address to Owner Mailing Address. If the property is owneroccupied, this field will contain an alpha 'O'.
        /// </remarks>
        [MaxLength(1)]
        public string OWN_OCPD { get; set; }

        /// <summary>
        /// A six-digit numeric code assigned by the U.S.Census Bureau to identify the area of the City where this address can be found.
        /// </summary>
        /// <remarks>
        /// The last two positions contain a suffix. Tract boundaries usually follow the center-lines of major streets or natural boundaries.
        /// 
        /// MPROP presently contains tracts assigned for the 2010 census.
        /// </remarks>
        public int? GEO_TRACT { get; set; }

        /// <summary>
        /// A three-digit code assigned by the U.S. Census Bureau to further subdivide Census Tract.
        /// </summary>
        /// <remarks>
        /// Generally, a census block conforms to City block boundaries.
        /// 
        /// MPROP presently contains blocks assigned for the 2010 census.
        /// </remarks>
        [MaxLength(4)]
        public string GEO_BLOCK { get; set; }

        /// <summary>
        /// Geographic area assigned by the U.S. Postal Services.
        /// </summary>
        /// <remarks>
        /// Normally GEO-ZIP-CODE has data in only the first five positions.
        /// </remarks>
        public int? GEO_ZIP_CODE { get; set; }

        /// <summary>
        /// An administrative area assigned to each district station.
        /// </summary>
        /// <remarks>
        /// Police districts are numbered 1-7.
        /// </remarks>
        public int? GEO_POLICE { get; set; }

        /// <summary>
        /// Current political area of the City identifying representation on the City of Milwaukee Common Council.
        /// </summary>
        public int? GEO_ALDER { get; set; }

        /// <summary>
        /// Previous political area of the City identifying representation of the City of Milwaukee Common Council.
        /// </summary>
        public int? GEO_ALDER_OLD { get; set; }

        /// <summary>
        /// Codes identifying the historic properties.
        /// </summary>
        /// <remarks>
        /// * 1: On the National Register of Historic Places (NRHP) and locally designated.
        /// * 2: On NRHP, but no local designation.
        /// * 3: Eligible for NRHP and locally designated.
        /// * 4: Eligible for NRHP, but no local designation.
        /// * 5: Not on NRHP, but locally designated.
        /// * b: (Blank) All other properties. 
        /// </remarks>
        [MaxLength(1)]
        public string HIST_CODE { get; set; }
    }
}
