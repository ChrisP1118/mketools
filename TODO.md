# Launch

* Relative times ("3 hours ago" on home page markers get out of date over time)
* Check for faulty import processes
* Hide "Properties" page?
* Crime/police dispatch call detail pages link cross-link
* Add "Developer" documentation/notes
  * List sources used
  * Mention API usage policy
* Add "About" page
* Add Google Analytics
* Document what's "major" and "minor"
* Bug: Slashes in policeDispatchCallType (e.g. /api/policeDispatchCallType/TRBL%20W/SUBJ)

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

# Minor Issues
* Caching headers - why Cache-Control: no-cache?

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
