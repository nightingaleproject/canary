import moment from 'moment';

// Generate HTML report from test result details
export default function report(test, name) {
    var html = "<style type=\"text/css\">.tab { margin-left: 40px; }; body { font-family: \"Arial, Helvetica, sans-serif\" };</style>"
    html += "<h1>Canary Test Results Report";
    if (name) {
        html += ` - ${name}`;
    }
    html += "</h1>";
    html += `<h3>Test performed at: ${moment(test["completedDateTime"]).format('YYYY-MM-DD HH:mm:ss')}</h3>`
    html += `<h3>Total Correct: ${test["correct"]}</h3>`
    html += `<h3>Total Errors: ${test["incorrect"]}</h3><br>`
    for (var category in test["results"]) {
        html += `<h2>${category}</h2>`;
        for (var property in test["results"][category]) {
            if (test["results"][category][property].hasOwnProperty("Match") && test["results"][category][property]["Match"] != null) {
                if (!!!test["results"][category][property]["Value"] ||
                    (Array.isArray(test["results"][category][property]["Value"]) && test["results"][category][property]["Value"].length === 0)) {
                    continue;
                }
                if (test["results"][category][property]["Match"] === "true") {
                    html += `<h3 style="color:#66CD00;">[CORRECT] ${property} (${test["results"][category][property]["Description"]})</h3>`;
                } else {
                    html += `<h3 style="color:#cc0000;">[ERROR] ${property} (${test["results"][category][property]["Description"]})</h3>`;
                }
                html += "<div class=\"tab\">";
                if (test["results"][category][property]["Type"] === "Dictionary" && test["results"][category][property]["Value"]) {
                    for (var field in test["results"][category][property]["Value"]) {
                        if (test["results"][category][property]["Value"][field]["Match"] === "true") {
                            html += `<h4 style="color:#66CD00;">[CORRECT] ${field}</h4>`;
                        } else {
                            html += `<h4 style="color:#cc0000;">[ERROR] ${field}</h4>`;
                        }
                        html += "<div class=\"tab\">";
                        html += `<b>Expected</b>: ${JSON.stringify(test["results"][category][property]["Value"][field]["Value"])}<br><br>`;
                        html += `<b>How Canary interpreted your input</b>: ${JSON.stringify(test["results"][category][property]["Value"][field]["FoundValue"])}<br>`;
                        html += "</div>";
                    }
                } else {
                    html += `<b>Expected</b>: ${JSON.stringify(test["results"][category][property]["Value"])}<br><br>`;
                    html += `<b>How Canary interpreted your input</b>: ${JSON.stringify(test["results"][category][property]["FoundValue"])}<br>`;
                }
                html += "</div>";
            }
        }
    }
    return html + "<br><br>";
}
