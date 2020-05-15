<template>
  <div>
    <page-title title="About This Site" />
    <hr />
    <b-alert show variant="info">This site is not affiliated with the City of Milwaukee or any official or government agency.</b-alert>
    <p>
      This site is designed to make data about crime in Milwaukee more accessible. It lets you see police and fire calls near you, receive email notifications
      whenever there are dispatch calls near you, and see general crime data.
    </p>
    <p>
      This site is also designed with other developers in mind. Milwaukee makes a lot of data publicly available, but it's not always easy to work with. All of
      the data used on this site is regularly updated from official City of Milwaukee sources, and is made available here via RESTful API in the hopes that other
      people will develop applications around this data.
    <p>
      <strong>Remember! An increased awareness of crime does not necessarily indicate an increase in crime.</strong>
      There is a general downward trend in major crimes in Milwaukee.
    </p>

    <h2>Origin</h2>
    <p>This site started when my wife and I decided to go for a walk one night. When we left our house, we discovered a young woman outside who had been robbed a few minutes earlier. While waiting for the police to arrive, we talked to her about what happened and discovered that the entire indicent had occurred directly in front of one of our security cameras. When the police arrived a few minutes later, we already had video footage of the suspects ready for them. This robbery had been the latest in a string of robberies that night, and this was the first photo or videos that police had managed to obtain of any of the incidents. If we hadn't spontaneously decided to go for a walk when we did, we likely never would've been aware of what had happened or been able to contribute to the police investigation.</p>
    <p>Thinking about the incident, I thought it'd be useful to have a site that would notify you if a situation like this occurred close to your home. This seems like it could be a valuable resource for others in the community, so I'm making this site free and available to anyone in Milwaukee.</p>
    <p>However, I remain somewhat conflicted about making this application available to anyone. All the data I'm using here is already available publicly. Nonetheless, the internet tends to cause people to distort data or skew facts to support their existing beliefs. By making it a lot easier for people to know about crimes taking place in their neighborhood, there's the risk of creating the impression that our city is becoming a more dangerous place. The data, however, shows the opposite -- crime is declining in Milwaukee.</p>
    <p>I hope to strike a balance with this site -- even as it makes people more aware of the crime in our community, I hope it can also better inform our citizens about general trends in crime in Milwaukee. If you have any ideas on how to better balance the way this data is presented, <b-link to="/about">feel free to get in touch</b-link>.</p>
    
    <h2>Contact</h2>
    <p>I always appreciate feedback -- if you have any, email me at cwilson at mkealerts.com.</p>
    
    <h2>Additional Resources</h2>
    <ul>
      <li><a href="https://city.milwaukee.gov/Directory/police/Crime-Maps-and-Statistics.htm" target="_blank">Milwaukee Crime Maps and Statistics</a> - this app has some great visualizations of crime trends in Milwaukee over time</li>
    </ul>

    <h2>Dispatch Call Types</h2>
    <p>More information on how dispatch call types are classified is available <b-link to="/dispatchCallTypes">here</b-link>.</p>
    
    <h2>Privacy Policy</h2>
    <p>
      I don't like spam, you don't like spam, no one likes spam. This site won't send you spam -- it'll only send you notifications emails to which you opt in. And every
      email has a link in it to unsubscribe you from that notification.
    </p>
    <p>
      This site uses Google Analytics to help me keep track of usage. If you sign in through Google or Facebook, I ask for the minimum permissions required, and don't
      use any data from them aside from your email address (only to send notifications you request).
    </p>

    <!-- <h2>Interesting Data</h2>
    <ul>
      <li>Properties
        <ul>
          <li><b-link to="/parcel?ownername1=CITY%20OF%20MILW">City of Milwaukee</b-link></li>
          <li><b-link to="/parcel?owneraddr=5225%20W%20VLIET%20ST">MPS</b-link></li>
          <li><b-link to="/parcel?ownername1=MILWAUKEE%20COUNTY%20PARKS">Milwaukee County Parks</b-link></li>
          <li><b-link to="/parcel?ownername1=BOARD%20OF%20REGENTS">University of Wisconsin</b-link></li>
          <li><b-link to="/parcel?descriptio=COUNTY">County-Owned Property</b-link></li>
          <li><b-link to="/parcel?descriptio=STATE">State-Owned Property</b-link></li>
          <li><b-link to="/parcel?descriptio=FEDERAL">Federally-Owned Property</b-link></li>
          <li><b-link to="/parcel?descriptio=MANUFACTURING">Manufacturing Land</b-link></li>
          <li><b-link to="/parcel?descriptio=COMMERCIAL">Commercial Land</b-link></li>
          <li><b-link to="/parcel?descriptio=AGRICULTURAL">Agricultural Land</b-link></li>
        </ul>
      </li>      
    </ul> -->

  </div>
</template>

<script>
export default {
  name: 'About',
  props: {
  }
}
</script>