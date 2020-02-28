appControllers.controller("PreviewController", function ($scope) {
    this.$inject = ['$scope'];

    $scope.source = {};
    $scope.ComponentType = ComponentType;
    $scope.DEFAULT_IMAGE_URL = BASE_URL + "/Content/images/transparent.png";
    $scope.PDF_IMAGE_URL = BASE_URL + "/Content/images/pdf-icon.png";

    $scope.getCurrentViewUrl = function () {

        if (!$scope.source.selectedSection.ID)
            return "";

        switch ($scope.source.selectedSection.ID) {
            case 1:
                return BASE_URL + "/app/partials/preview/primary-banner.html";
            case 2:
                return BASE_URL + "/app/partials/preview/details-page.html";
            case 3:
                return BASE_URL + "/app/partials/preview/recommended-accounts.html";
            case 4:
                return BASE_URL + "/app/partials/preview/all-offers.html";
            case 5:
                return BASE_URL + "/app/partials/preview/splash-page.html";
            case 6:
                return BASE_URL + "/app/partials/preview/sign-off-page.html";
            case 7:
                return BASE_URL + "/app/partials/preview/cc.html";
            case 8:
                return BASE_URL + "/app/partials/preview/deposits.html";
            case 9:
                return BASE_URL + "/app/partials/preview/equity.html";
            case 10:
                return BASE_URL + "/app/partials/preview/certificates.html";
            case 11:
                return BASE_URL + "/app/partials/preview/bulletin-zone.html";
            case 12:
                return BASE_URL + "/app/partials/preview/hero-offer.html";
            case 13:
                return BASE_URL + "/app/partials/preview/edu-offer.html";
            case 14:
                return BASE_URL + "/app/partials/preview/prod-offer.html";
            case 15:
                return BASE_URL + "/app/partials/preview/pwm-bulletin.html";
            case 16:
                return BASE_URL + "/app/partials/preview/pwm-details-page.html";
            case 17:
                return BASE_URL + "/app/partials/preview/pwm-primary-banner.html";
            case 18:
                return BASE_URL + "/app/partials/preview/view-all.html";
            case 19:
                return BASE_URL + "/app/partials/preview/olb-bulletin.html";
            case 20:
                return BASE_URL + "/app/partials/preview/ghost-offer.html";
            case 21:
                return BASE_URL + "/app/partials/preview/olb-learn-more.html";
            case 22:
                return BASE_URL + "/app/partials/preview/rto-primary-banner.html";
            case 23:
                return BASE_URL + "/app/partials/preview/rto-details-page.html";
        }
        return "";
    };

    $scope.renderSection = function () {
        if ($scope.source.selectedSection.ID)
            return true;
        return false;
    };

    $scope.isEmptyOrWhiteSpace = function(string) {
        if (string && string.trim() != '') {
            return false;
        }
        return true;
    }

    $scope.isVisible = function (typeID) {
        var component = $scope.getComponent(typeID);
        if (!component || component.Inactive)
            return false;

        switch (component.TypeID) {
            case $scope.ComponentType.CALL_TO_ACTION:
                if (!component.Inactive) {
                    if (component.Data.buttonText && component.Data.buttonText !== '')
                        return true;
                    else if ($scope.getHref(typeID) !== "")  {
                        return true;
                    }
                    else if (component.Data.selectedOption && component.Data.selectedOption.Value !== '') {
                        return true;
                    }
                }
                return false;
            case $scope.ComponentType.CALL_TO_ACTION_2:
                if (!component.Inactive) {
                    if (component.Data.buttonText && component.Data.buttonText !== '')
                        return true;
                    else if ($scope.getHref(typeID) !== "")  {
                        return true;
                    }
                    else if (component.Data.selectedOption && component.Data.selectedOption.Value !== '' && component.Data.selectedOption.Value !== 'NULL') {
                        return true;
                    }
                }
                return false;
            case $scope.ComponentType.REMINDER:
            case $scope.ComponentType.OFFER_REJECTION:
                if (!component.Inactive && (component.Data.selectedOption.Value && component.Data.selectedOption.Value !== 'NULL'))
                    return true;
                break;
            case $scope.ComponentType.DISCLAIMER:
            case $scope.ComponentType.DETAILS:
                if (!component.Inactive && (component.Data.Description && component.Data.Description !== ''))
                    return true;
                break;
            case $scope.ComponentType.TERMS_CONDITIONS:
                if (!component.Inactive && (component.Data.Conditions && component.Data.Conditions !== ''))
                    return true;
                break;
            case $scope.ComponentType.CLICK_TO_CHAT:
                return !component.Inactive;
                break;
        }

        return false;
    };

    $scope.getComponent = function (typeID) {
        console.log("self.getComponent ", typeID, $scope.source.getComponent(typeID));
        return $scope.source.getComponent(typeID);
    };

    $scope.getSrc = function (typeID) {
        var DEFAULT = $scope.DEFAULT_IMAGE_URL;

        var component = $scope.getComponent(typeID);
        if (!component || !component.Data.src)
            return DEFAULT;

        if (!component.Data.isExternal) {
            return BASE_URL + component.Data.src;
        } else {
            return component.Data.src;
        }

        return DEFAULT;
    };

    $scope.getCTALink = function (typeID) {
        var component = $scope.getComponent(typeID);
        if (!component)
            return false;

        if (component && component.Data.url && component.Data.selectedOption && component.Data.selectedOption.Value === "CustomURL") {
            if (component.Data.url.indexOf("http://") < 0 && component.Data.url.indexOf("https://") < 0) {
                return "http://" + component.Data.url;
            } else {
                return component.Data.url;
            }
        } else if (component && component.Data.selectedOption) {
            return component.Data.selectedOption.Name;
        }

        return "";
    }

    $scope.isALink = function (typeID) {
        var text = $scope.getCTALink(typeID);
        if (text.indexOf("http://") < 0 && text.indexOf("https://") < 0)
            return false;

        return true;
    }

    $scope.getHref = function (typeID) {
        var component = $scope.getComponent(typeID);
        if (!component)
            return false;

        if (component && component.Data.url && component.Data.selectedOption && component.Data.selectedOption.Value === "CustomURL") {
            if (component.Data.url.indexOf("http://") < 0 && component.Data.url.indexOf("https://") < 0) {
                return "http://" + component.Data.url;
            } else {
                return component.Data.url;
            }
        }
        return "";
    };

    $scope.display = function (typeID) {
        var component = $scope.getComponent(typeID);
        if (!component)
            return "";

        switch (typeID) {
            case $scope.ComponentType.MAIN_IMAGE:
                return $scope.getSrc(typeID);
                break;
            case $scope.ComponentType.CALL_TO_ACTION:
                if (component.Data.buttonText && component.Data.buttonText !== "")
                    return component.Data.buttonText;
                else {
                    var href = $scope.getHref(typeID);
                    if (href && href !== "") {
                        return href;
                    }
                }
                break;
            case $scope.ComponentType.CALL_TO_ACTION_2:
                if (component.Data.buttonText && component.Data.buttonText !== "")
                    return component.Data.buttonText;
                else {
                    var href = $scope.getHref(typeID);
                    if (href && href !== "") {
                        return href;
                    }
                }
                break;
            case $scope.ComponentType.DETAILS:
                return component.Data.Description;
                break;
            case $scope.ComponentType.DISCLAIMER:
                return component.Data.Description;
                break;
            case $scope.ComponentType.REMINDER:
                return component.Data.selectedOption.Name;
                break;
            case $scope.ComponentType.OFFER_REJECTION:
                return component.Data.selectedOption.Name;
                break;
            case $scope.ComponentType.HEADLINE:
                return component.Data.Headline;
                break;
            case $scope.ComponentType.TERMS_CONDITIONS:
                return component.Data.Conditions;
                break;
        }

        return "";
    };

    $scope.init = function (source) {
        if (source)
            $scope.source = source;
    };
});