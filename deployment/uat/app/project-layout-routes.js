app.config(['$routeProvider',
    function ($routeProvider) {
        $routeProvider.
          when('/layout-menu/:projectID', {
              templateUrl: BASE_URL + '/app/views/layout-menu.html',
              controller: 'ProjectLayoutMenuController'
          }).
          when('/primary-banner/:projectID', {
              templateUrl: BASE_URL + '/app/views/primary-banner.html',
              controller: 'SectionController'
          }).
          when('/details-page/:projectID', {
              templateUrl: BASE_URL + '/app/views/details-page.html',
              controller: 'SectionController'
          }).
          when('/recommended-accounts/:projectID', {
              templateUrl: BASE_URL + '/app/views/recommended-accounts.html',
              controller: 'SectionController'
          }).
          when('/all-offers/:projectID', {
              templateUrl: BASE_URL + '/app/views/all-offers.html',
              controller: 'SectionController'
          }).
          when('/splash-page/:projectID', {
              templateUrl: BASE_URL + '/app/views/splash-page.html',
              controller: 'SectionController'
          }).
          when('/sign-off-page/:projectID', {
              templateUrl: BASE_URL + '/app/views/sign-off-page.html',
              controller: 'SectionController'
          }).
          when('/credit-cards/:projectID', {
              templateUrl: BASE_URL + '/app/views/credit-cards.html',
              controller: 'SectionController'
          }).
          when('/deposits/:projectID', {
              templateUrl: BASE_URL + '/app/views/deposits.html',
              controller: 'SectionController'
          }).
          when('/equity/:projectID', {
              templateUrl: BASE_URL + '/app/views/equity.html',
              controller: 'SectionController'
          }).
            when('/certificates/:projectID', {
                templateUrl: BASE_URL + '/app/views/certificates.html',
                controller: 'SectionController'
            }).
            when('/bulletin-zone/:projectID', {
                templateUrl: BASE_URL + '/app/views/bulletin-zone.html',
                controller: 'SectionController'
            }).
          otherwise({
              redirectTo: '/layout-menu/:projectID'
          });
    }]);


