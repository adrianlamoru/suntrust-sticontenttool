var appServices = angular.module('appServices', []);
var appControllers = angular.module('appControllers', ['appServices', 'textAngular', 'ngSanitize']);
var appDirectives = angular.module('appDirectives', ['appServices']);

var app = angular.module('app', ['ngRoute', 'ngSanitize', 'appControllers', 'appServices', 'appDirectives', 'angularFileUpload']);

var timeouts = [];

appServices.factory('spinnerHttpInterceptor', function ($q, $window) {
    var spinner = document.getElementById("spinner");
    return function (promise) {
        return promise.then(function (response) {
            if (spinner) {
                clearTimeout(timeouts.shift());
                if (timeouts.length == 0) {
                    spinner.className = "spinner";
                }
                
            }
            return response;
        }, function (response) {
            if (spinner) {
                clearTimeout(timeouts.shift());

                if (timeouts.length == 0) {
                    spinner.className = "spinner";
                }
            }
            return $q.reject(response);
        });
    }
});

appServices.config(function ($httpProvider) {
    $httpProvider.responseInterceptors.push('spinnerHttpInterceptor');

    var spinnerFunction = function spinnerFunction(data, headersGetter) {
        //clearTimeout(timeout);
        timeouts.push(setTimeout(function () {
            var spinner = document.getElementById("spinner");
            spinner.className = "spinner loading";
        }, 2000));

        return data;
    };

    $httpProvider.defaults.transformRequest.push(spinnerFunction);
});

var TAG_CMD_ID = 'insertCommand';
var SYM_CMD_ID = 'sym';

/*
appControllers.config(['$provide', function ($provide) {
    $provide.decorator('taOptions', ['taRegisterTool', '$delegate', function (taRegisterTool) {
          var me = this;
        
          // Now add the button to the default toolbar definition
          // Note: It'll be the last button
          //taOptions.toolbar[3].push('customInsertImage');
          return taOptions;
      }
      ]);
  }
]);

*/