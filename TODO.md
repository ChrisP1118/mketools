# Launch

* Filtered table - refreshData called twice in quick succession when bounds change -- debounce this?
* Clean up: "Exceptions" folder vs "Middleware\Exceptions" folder
* Crime page - filters lost after clicking through to detail page
* Confirm email address before sending notifications?
* Reset password/my profile page
* Test SSO with prod URLs
* On "Nearby Map", we end up with properties overlapping when it's the same parcel -- like condo buildings
* Better map icons/markers
* Crime/police dispatch call detail pages link cross-link
* Add some error handling inside of jobs
* Document what's "major" and "minor"
* EntityWriteService logger should use service type, not entity, as generic
* Send admin alert when downloads/imports fail (parcels, mprop, etc.)
* Add AsNoTracking to EF Core queries?
* Crime data import can be sped up -- regularly, only import things in last month; on a weekly basis do a full import

# Minor Issues
* Overflow-x: scroll on filtered table container
* On mobile, default filtered table map to top; desktop to right
* Caching headers - why Cache-Control: no-cache?
* When we're loading a "polygon" geometry, we should use the centroid instead of the first corner
* Update page meta tags (title, description, etc.) on route changes: https://alligator.io/vuejs/vue-router-modify-head/
* When importing crime data, we probably don't need to update/upsert any older than a certain span (or maybe do a complete import on a weekly/monthly basis, but just the last x weeks on a daily basis)
* Crime data locations are block-based, not address-based

# Backlog

* Open keyword search on table (for example: property search across owner fields)
  * Filtering isn't clear on property page (HOUSE_NR_HI vs LO)
* Load table filters from URL (and link from info windows?)
* Add "share" link on filtered table that creates a link with URL parameters for filter
* Add way to lookup crime rate relative to specific address
* Add trash day alerts (import users from MkeTrashDay)
* PWA
* PWA - add notifications API support
* Add fire history
* Add traffic accident data?
* Crime notifications
* Clear out status on dispatch calls after awhile?
* Add GraphQL support
* Add TAXKEY (and property) for crimes?
* Add TAXKEY (and property) for dispatch calls -- might have multiple properties nearby?
* Regular, automated dispatch call data export to static file (available for download)

# Done

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
