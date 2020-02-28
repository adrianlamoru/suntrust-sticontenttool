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
            sectionName: "Ghost"
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
        },
        {
            sectionType: SectionType.PWM_HERO_OFFR,
            sectionName: "Hero Offer"
        },
        {
            sectionType: SectionType.PWM_EDU_OFFR,
            sectionName: "Resource Center"
        },
        {
            sectionType: SectionType.PWM_SPECIAL_OFFR,
            sectionName: "Special Offer"
        },
        {
            sectionType: SectionType.PWM_BULLETIN,
            sectionName: "PWM Bulletin"
        },
        {
            sectionType: SectionType.PMW_LEARN_MORE,
            sectionName: "PWM Offer Details"
        },
        {
            sectionType: SectionType.PRIMARY_OFFR,
            sectionName: "OLB Primary Banner"
        },
        {
            sectionType: SectionType.VIEW_ALL,
            sectionName: "OLB View All"
        },
        {
            sectionType: SectionType.PWM_BULLETIN_ZONE,
            sectionName: "OLB Bulletin Zone"
        },
        {
            sectionType: SectionType.GHOST_OFFR,
            sectionName: "OLB Ghost"
        },
        {
            sectionType: SectionType.LEARN_MORE,
            sectionName: "OLB Offer Details"
        },
        {
            sectionType: SectionType.RTO_PRIMARY_OFFR,
            sectionName: "Primary Banner"
        },
        {
            sectionType: SectionType.RTO_LEARN_MORE,
            sectionName: "Offer Details"
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