# Launch

* Password reset
* Test SSO with prod URLs
* Change "Contact" page to "Support"
* Hide "Properties" page?
* Crime/police dispatch call detail pages link cross-link
* Add "Developer" documentation/notes
  * List sources used
  * Mention API usage policy
* Add "About" page
* Add Google Analytics
* Document what's "major" and "minor"
* Add some error handling inside of jobs
* Clean up Swagger spec (especially details/remarks at top)
* Add "share" link on filtered table that creates a link with URL parameters for filter
* On "Nearby Map", we end up with properties overlapping when it's the same parcel -- like condo buildings

# Minor Issues
* Caching headers - why Cache-Control: no-cache?
* When we're loading a "polygon" geometry, we should use the centroid instead of the first corner

# Backlog

* Add way to lookup crime rate relative to specific address
* Can we consolidate/cache the Google Maps instance to cut down on API requests? Or is it only counting as one API request right now?
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
