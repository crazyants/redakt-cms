import {Aurelia} from 'aurelia-framework';
import {bootstrap} from 'aurelia-bootstrapper-webpack';

import 'bootstrap';

import '../node_modules/bootstrap/dist/css/bootstrap.css';
import '../assets/css/core.css';
import '../assets/css/components.css';
import '../assets/css/icons.css';
import '../assets/css/pages.css';
import '../assets/css/menu.css';
import '../assets/css/responsive.css';

bootstrap((aurelia: Aurelia): void => {
  aurelia.use
    .standardConfiguration()
    .developmentLogging();

  aurelia.start().then(() => aurelia.setRoot('app', document.body));

  //const rootElem = document.body;
  //aurelia.start().then(() => aurelia.setRoot('app', rootElem));
  //rootElem.setAttribute('aurelia-app', '');
});
