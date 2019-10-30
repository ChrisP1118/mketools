<template>
  <div>
    <page-title title="About This Site" />
    <p>
      This site is designed to make data about crime in Milwaukee more accessible. It lets you see police and fire calls near you, receive email notifications
      whenever there are dispatch calls near you, and see general crime data.
    </p>
    <p>
      <strong>Remember! An increased awareness of crime does not necessarily indicate an increase in crime.</strong>
      There is a general downward trend in major crimes in Milwaukee.
    </p>
    <h2>Origin</h2>
    <p>This site started when my wife and I decided to go for a walk one night. When we left our house, we discovered a young woman outside who had been robbed a few minutes earlier. While waiting for the police to arrive, we talked to her about what happened and discovered that the entire indicent had occurred directly in front of one of our security cameras. When the police arrived a few minutes later, we already had video footage of the suspects ready for them. This robbery had been the latest in a string of robberies that night, and this was the first photo or videos that police had managed to obtain of any of the incidents. If we hadn't spontaneously decided to go for a walk when we did, we likely never would've been aware of what had happened or been able to contribute to the police investigation.</p>
    <p>Thinking about the incident, I thought it'd be useful to have a site that would notify you if a situation like this occurred close to your home. This seems like it could be a valuable resource for others in the community, so I'm making this site free and available to anyone in Milwaukee.</p>
    <p>However, I remain somewhat conflicted about making this application available to anyone. All the data I'm using here is already available publicly. Nonetheless, the internet tends to cause people to distort data or skew facts to support their existing beliefs. By making it a lot easier for people to know about crimes taking place in their neighborhood, there's the risk of creating the impression that our city is becoming a more dangerous place. The data, however, shows the opposite -- crime is declining in Milwaukee.</p>
    <p>I hope to strike a balance with this site -- even as it makes people more aware of the crime in our community, I hope it can also better inform our citizens about general trends in crime in Milwaukee. If you have any ideas on how to better balance the way this data is presented, <b-link to="/support">feel free to get in touch</b-link>.</p>
    <h2>Additional Resources</h2>
    <ul>
      <li><a href="https://city.milwaukee.gov/Directory/police/Crime-Maps-and-Statistics.htm" target="_blank">Milwaukee Crime Maps and Statistics</a> - this app has some great visualizations of crime trends in Milwaukee over time</li>
    </ul>    
  </div>
</template>

<script>
export default {
  name: 'About',
  props: {
  }
}
</script>