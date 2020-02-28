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
            when('/hero-offer/:projectID', {
                templateUrl: BASE_URL + '/app/views/hero-offer.html',
                controller: 'SectionController'
            }).
            when('/edu-offer/:projectID', {
                templateUrl: BASE_URL + '/app/views/edu-offer.html',
                controller: 'SectionController'
            }).
            when('/prod-offer/:projectID', {
                templateUrl: BASE_URL + '/app/views/prod-offer.html',
                controller: 'SectionController'
            }).
            when('/pwm-bulletin/:projectID', {
                templateUrl: BASE_URL + '/app/views/pwm-bulletin.html',
                controller: 'SectionController'
            }).
            when('/pwm-details/:projectID', {
                templateUrl: BASE_URL + '/app/views/pwm-details-page.html',
                controller: 'SectionController'
            }).
            when('/pwm-primary-bann/:projectID', {
                templateUrl: BASE_URL + '/app/views/pwm-primary-banner.html',
                controller: 'SectionController'
            }).
            when('/view-all/:projectID', {
                templateUrl: BASE_URL + '/app/views/view-all.html',
                controller: 'SectionController'
            }).
            when('/olb-bulletin/:projectID', {
                templateUrl: BASE_URL + '/app/views/olb-bulletin.html',
                controller: 'SectionController'
            }).
            when('/ghost-offer/:projectID', {
                templateUrl: BASE_URL + '/app/views/ghost-offer.html',
                controller: 'SectionController'
            }).
            when('/olb-learn-more/:projectID', {
                templateUrl: BASE_URL + '/app/views/olb-learn-more.html',
                controller: 'SectionController'
            }).
            when('/rto-primary-bann/:projectID', {
                templateUrl: BASE_URL + '/app/views/rto-primary-banner.html',
                controller: 'SectionController'
            }).
            when('/rto-details/:projectID', {
                templateUrl: BASE_URL + '/app/views/rto-details-page.html',
                controller: 'SectionController'
            }).
          otherwise({
              redirectTo: '/layout-menu/:projectID'
          });
    }]);