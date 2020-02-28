appControllers.controller("LayoutsController", function ($scope, $location) {
    $scope.projectID = projectID;
    $scope.project = {};
    $scope.SectionType = SectionType;
    $scope.renderModel = [
        {
            sectionType: SectionType.PRIMARY_BANNER,
            sectionName: "Primary Banner"
        },
        {
            sectionType: SectionType.DETAILS_PAGE,
            sectionName: "Offers Details"
        },
        {
            sectionType: SectionType.RECOMMENDED_ACCOUNT,
            sectionName: "Recommended Accounts"
        },
        {
            sectionType: SectionType.ALL_OFFERS,
            sectionName: "All Offers"
        },
        {
            sectionType: SectionType.SPLASH_PAGE,
            sectionName: "Splash Page"
        },
        {
            sectionType: SectionType.SIGN_OFF_PAGE,
            sectionName: "Sign Off Page"
        },
        {
            sectionType: SectionType.CREDIT_CARDS,
            sectionName: "New Account Center"
        },
        {
            sectionType: SectionType.DEPOSITS,
            sectionName: "New Account Center"
        },
        {
            sectionType: SectionType.EQUITY,
            sectionName: "New Account Center"
        },
        {
            sectionType: SectionType.CD,
            sectionName: "New Account Center"
        },
        {
            sectionType: SectionType.BULLETIN_ZONE,
            sectionName: "Bulletin Zone"
        }
    ];

    $scope.getViewModel = function (project, layoutID, sectionID) {
        return new PreviewViewModel(project, layoutID, sectionID);
    };

    $scope.init = function () {
        if (projectID > 0) {
            $scope.project = project;
        }
    };

    $scope.init();
});