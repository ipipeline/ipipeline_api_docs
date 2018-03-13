dundas.embed.logon.getLogonSession = function(n, t, i) {
    var e = function(n) {
            return n.replace(/\/$/, "")
        },
        r = new XMLHttpRequest,
        u = e(n) + "/API/LogOn/",
        f;
    r.open("POST", u, !0);
    r.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    r.setRequestHeader("Accept", "application/json");
    u.toLowerCase().startsWith("https://") && (dundas.embed.logon.disableWithCredentialsFlag || (r.withCredentials = "true"));
    r.onload = function() {
        if (r.status === 200) {
            var n = JSON.parse(r.responseText);
            i && i(n)
        }
    };
    f = Object.keys(t).map(function(n) {
        return encodeURIComponent(n) + "=" + encodeURIComponent(t[n])
    }).join("&");
    debugger;
    r.send(f)
};

/*
http://www.dundas.com/support/api-docs/rest/#Dundas%20BI%20REST%20API%20Reference/Session/CustomAttributes/%7Bid%7D/POST.html%3FTocPath%3DDundas%20BI%20REST%20API%20Reference%7CSession%7C_____5

http://www.dundas.com/support/api-docs/NET/#html/T_Dundas_BI_WebApi_Models_CustomAttributeData.htm
*/ 
dundas.embed.logon.setCustomAttributes = function(n,t,i) 
{
    var e = function(n) {
        return n.replace(/\/$/, "")
    },
    r = new XMLHttpRequest,
    u = e(n) + "/API/Session/CustomAttributes/"+t.currentSession+"/?sessionId="+t.NewSession+"\"",
    f;
    r.open("POST", u, !0);
    r.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    r.setRequestHeader("Accept", "application/json");
    u.toLowerCase().startsWith("https://") && (dundas.embed.logon.disableWithCredentialsFlag || (r.withCredentials = "true"));
    r.onload = function() {
        if (r.status === 200) {
            var n = JSON.parse(r.responseText);
            i && i(n)
        }
    };
    f = Object.keys(t).map(function(n) {
        return encodeURIComponent(n) + "=" + encodeURIComponent(t[n])
    }).join("&");
    r.send(f)
};
