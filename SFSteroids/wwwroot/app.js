export async function beforeStart(options, extensions, blazorBrowserExtension) {
    if (blazorBrowserExtension.BrowserExtension.Mode === blazorBrowserExtension.Modes.ContentScript) {
        if (window.location.href.indexOf('sfgame.net') !== -1 || window.location.href.indexOf('localhost') !== -1) {
            // Set timeout to load after the webgl init - otherwise the body content is discarded
            await (new Promise(resolve => setTimeout(resolve, 1000)));
            const appDiv = document.createElement("div");
            appDiv.id = "SFSteroids";
            document.body.appendChild(appDiv);
        }
    }
}
