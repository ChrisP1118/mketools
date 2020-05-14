# Launch
* Improvement: Should we limit geocoding results to Milwaukee? Or add a way of specifying a municipality?
* Improvement: Import MPROP data (and override MCLIO parcels as needed)
* Doc: Document what's "major" and "minor"
* Prep: Convert MkeTrashDay accounts

# Minor Issues - Front-End
* Fix: Overlays are wrong (after partial map reload?) on nearby map (but fine on other maps?)
* Fix: Crime/List page - filters lost after clicking through to detail page
* Fix: Properties View page still only shows a single property per common parcel (ideally, we'd be able to use a function when displaying an info window that could make an API call to load all properties for the common parcel)
* Fix: "overflow-x: scroll" on filtered table container
* Fix: Caching headers - why Cache-Control: no-cache?
* Improvement: Crime/police dispatch call detail pages link cross-link
* Improvement: Properties/List - add additional table fields
* Improvement: CommonParcels map instead of Properties?
* Improvement: Better map icons/markers
* Improvement: When we're loading a "polygon" geometry, we should use the centroid instead of the first corner

# Minor Issues - Back-End
* Verify: Facebook, Google accounts
* Verify: Log retention policy
* Fix: Automatically call script that creates dispatch types (as an EF migration?)
* Improvement: Confirm email address before sending notifications?
* Improvement: Change bulk import batch size (from 100 to ??)
* Improvement: Cache StreetReferences data? (Or add some indexes to speed them up?)
* Improvement: Better error handling/logging (error alert emails)
* Improvement: Reset password/my profile page

# Backlog
* Improvement: Download MCLIO datasets
* Epic: Property assessment viewer, with notifications on property owner changes
* Improvement: Open keyword search on table (for example: property search across owner fields)
  * Filtering isn't clear on property page (HOUSE_NR_HI vs LO)
* Improvement: Load table filters from URL (and link from info windows?)
* Improvement: Add "share" link on filtered table that creates a link with URL parameters for filter
* Epic: Add way to lookup crime rate relative to specific address
* Epic: PWA - add notifications API support
* Epic: Add fire history
* Epic: Add traffic accident data?
* Epic: Add GraphQL support
* Add TAXKEY (and property) for crimes?
* Add TAXKEY (and property) for dispatch calls -- might have multiple properties nearby?
* Epic: Regular, automated dispatch call data export to static file (available for download)

# Done
* ~~Check: Do we have some 9-digit TAXKEYs (Excel removed the leading zero?)~~
* ~~Check: mprop2019 seems to be funky~~
* ~~Improvement: Property list (change to parcel list?)~~
* ~~Improvement: GeocodeItemsJob: Save geocoded items~~
* ~~Improvement: GeocodeItemsJob: Better batching (don't want the entire job on a single EF content)~~
* ~~Log API response times?~~
* ~~Improvement: Better health checks~~
* ~~Improvement: Add indexes for address fields to speed this geocoding~~
* ~~Improvement: Refresh map on home page periodically~~
* ~~Improvement: Hangfire Dashboard~~
* ~~Fix: Split crime (WIBR) data into two jobs (historical and current)~~
* ~~Fix: Take a look at the StringReference table that EF is creating~~
* ~~Improvement: Upgrade to .NET Core 3.1~~
* ~~Requirement: Unsubscribe link for pickup dates notifications~~
* ~~Fix: Better use of bounds index on geographic filtering~~
* ~~Improvement: Add AsNoTracking to EF Core queries?~~
* ~~Epic: Add trash day alerts (import users from MkeTrashDay)~~
* ~~Use Vuex~~
* ~~Add page for "Crimes"~~
* ~~Manage notifications and account~~
* ~~Add fire dispatch calls~~
* ~~Download data sources~~
* ~~Automated data source imports (Hangfire)~~
* ~~Create a DTO for IGeometry types~~
* ~~Auth with Google, Facebook~~
* ~~Consolidate login/register response handlers into a mixin~~
* ~~Use Vuex on Home page dispatch call list/map markers~~
* ~~Refresh home page on schedule (to show new markers)~~
* ~~Use police dispatch call types~~
* ~~Use fire dispatch call types~~
* ~~Analysis on failed geocode requests - how can we get more hits?~~
* ~~Consistent mapping icons (based on call types)~~
* ~~Add database indexes (ReportedDateTime on Police/FireDispatchCalls)~~
* ~~Dispatch call notifications~~
* ~~Individual/detail pages for dispatch calls, crimes~~
* ~~Bug: Slashes in policeDispatchCallType (e.g. /api/policeDispatchCallType/TRBL%20W/SUBJ)~~
* ~~Health check -- watch for faulty import processes~~
* ~~Add loading indicator to maps (the lag is especially noticeable on Crime page when filtering based on map)~~
* ~~Checking the box for "Filter based on map" should refresh~~
* ~~Show friendlier dates in dispatch call list (all tables?)~~
* ~~Relative times ("3 hours ago" on home page markers get out of date over time)~~
* ~~Smaller range of distances for alerts~~
* ~~Date filter doesn't work correctly on filtered table (it's doing equality on the date, rather than a greater/less than)~~
* ~~Create component for nearby properties map~~
* ~~If the page loads on a dispatch page, and then you switch over to home, no dispatch calls are loaded on the home page map~~
* ~~Change "Contact" page to "Support"~~
* ~~The call to getIcon should include the full URL (maybe even the entire Icon object?)~~
* ~~FilteredTableMap: Rename "getItemMarkerGeometry" to "getItemMarkerPosition"~~
* ~~Password reset~~
* ~~Clean up Swagger spec (especially details/remarks at top)~~
* ~~Create README.MD file~~
* ~~Add "Developer" documentation/notes~~
  * ~~List sources used~~
  * ~~Mention API usage policy~~
* ~~Add "About" page~~
* ~~Error messages when log in fails~~
* ~~Loading message when performing initial geolookup~~
* ~~Navbar icon is skewed on mobile~~
* ~~Add Google Analytics~~
* ~~Show errors if OAuth fails~~
* ~~Error messages when logging in/creating account that's tied to external provider ("That email is associated with Facebook...")~~
* ~~Remove "Loading data" message on filtered table (or at least make it not interfere with user typing)~~
* ~~Add descriptions to each page -- e.g. police dispatch call vs. crime data ("Not all police calls are crimes ... and vice versa") -- maybe link to data sources?~~
* ~~Add unsubscribe link to notification emails~~
* ~~Filtered table map issues~~
  * ~~Changing position (top vs. left)~~
  * ~~When map on top, table column headers are hidden~~
  * ~~Can we get rid of top/right option?~~
  * ~~Zooming out~~
* ~~Properties list - more data in info window~~
* ~~Properties detail/view page~~
* ~~Base nearby map location (and loaded properties) on map bounds (and only show properties at a certain zoom level)~~
* ~~Is the ImportCrimes job running correctly on schedule? I think so - just keep monitoring it~~
* ~~Crime\List - needs better info windows~~
* ~~THEFT appears as non-crime? https://localhost:5001/#/policeDispatchCall/192931850~~
* ~~Health check logic is backwards~~
* ~~Add some error handling inside of jobs~~
* ~~Fix casing with some of the JSON property names (e.g. "p_A_TOTAL")~~
* ~~On "Nearby Map", we end up with properties overlapping when it's the same parcel -- like condo buildings~~
* ~~Send admin alert when downloads/imports fail (parcels, mprop, etc.)~~
* ~~On mobile, default filtered table map to top; desktop to right~~
* ~~Fix: "Exceptions" folder vs "Middleware\Exceptions" folder~~
* ~~Fix: EntityWriteService logger should use service type, not entity, as generic~~
* ~~Improvement: Abbreviated reverse geocode (currently returns ~267KB of data or more -- all we need is an address string)? Or add an "includes" property to this? (Switched to using a DTO for geocode results)~~
* ~~Fix: Filtered table - refreshData called twice in quick succession when bounds change -- debounce this?~~
* ~~Improvement: In Swagger docs, add links to documentation on city site~~
* ~~Test: SSO with prod URLs~~
* ~~Improvement: Add link to dispatch call in email notification~~
