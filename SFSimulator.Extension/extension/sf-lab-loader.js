sfLabLoad();

async function sfLabLoad() {
  const doc = (document.head || document.documentElement);
  const styles = document.createElement('link');
  styles.rel = 'stylesheet';
  styles.href = chrome.runtime.getURL('./styles.css');
  doc.appendChild(styles);

  const bootstrap = document.createElement('sf-lab');
  bootstrap.setAttribute( 'extension-base-url', chrome.runtime.getURL(''));
  document.body.appendChild(bootstrap);

  console.log('Before Angular:', bootstrap.outerHTML);

  const main = document.createElement('script');
  main.type = 'module';
  main.src = chrome.runtime.getURL('./main.js');
  setTimeout(function () {
    doc.appendChild(main);
  }, 300);
}
