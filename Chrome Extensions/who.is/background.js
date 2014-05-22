// Detect our omnibox keyword ("keyword" : "whois") and then forward to the correct page accordingly.
chrome.omnibox.onInputEntered.addListener(
	function(text){
		console.log('inputEntered: ' + text);
		chrome.tabs.update(null, {
			url:  "http://who.is/whois/" + text
		});
	}
);