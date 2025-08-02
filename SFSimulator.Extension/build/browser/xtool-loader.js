// TODO: we might need to remove this if we can register the files in the manifest.json
xtoolLoad();

async function xtoolLoad() {
  let doc = (document.head || document.documentElement);
  let styles = document.createElement('link');
  styles.rel = 'stylesheet';
  styles.href = chrome.runtime.getURL('./styles.css');
  doc.appendChild(styles);

  let bootstrap = document.createElement('x-tool');
  document.body.appendChild(bootstrap);

  let main = document.createElement('script');
  main.type = 'module';
  main.src = chrome.runtime.getURL('./main.js');
  setTimeout(function () {
    doc.appendChild(main);
  }, 300);

}
