module.exports = config => config.set({
  frameworks: ['jasmine', '@angular-devkit/build-angular'],
  plugins: [require('karma-jasmine'), require('karma-chrome-launcher'), require('@angular-devkit/build-angular/plugins/karma')],
  reporters: ['progress'], browsers: ['ChromeHeadless'], singleRun: true,
  port: 19877, captureTimeout: 180000, browserNoActivityTimeout: 180000,
  browserDisconnectTimeout: 30000, browserDisconnectTolerance: 1,
  client: { clearContext: false }
});
