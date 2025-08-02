// TODO: we might need to remove this if we can register the files in the manifest.json
xtoolLoad();

function xtoolLoad() {
  let doc = (document.head || document.documentElement);
  let style = document.createElement('link');
  style.rel = 'stylesheets';
  style.href = chrome.runtime.getURL('./styles.css');
  doc.appendChild(style);

  let icons = document.createElement('script');
  icons.type = 'module';
  icons.src = 'https://kit.fontawesome.com/1e4de889d4.js'
  icons.crossOrigin = 'anonymous';
  doc.appendChild(icons);

  let bootstrap = document.createElement('div');
  bootstrap.innerHTML = '<x-tool></x-tool>'
  document.body.appendChild(bootstrap);

  let runtime = document.createElement('script');
  runtime.type = 'module';
  runtime.src = chrome.runtime.getURL('./runtime.js');
  setTimeout(function () {
    doc.appendChild(runtime);
  }, 300);

  let polyfills = document.createElement('script');
  polyfills.type = 'module';
  polyfills.src = chrome.runtime.getURL('./polyfills.js');
  setTimeout(function () {
    doc.appendChild(polyfills);
  }, 600);

  let main = document.createElement('script');
  main.type = 'module';
  main.src = chrome.runtime.getURL('./main.js');
  setTimeout(function () {
    doc.appendChild(main);
  }, 900);
}
