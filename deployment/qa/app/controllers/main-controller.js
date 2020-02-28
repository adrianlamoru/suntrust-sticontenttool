appControllers.controller('MainController', function ($scope, $rootScope, $location, CommonService) {

    this.$inject = ['$scope', 'CommonService'];

    $scope.isCreateProjectDlgVisible = false;
    $scope.isEditOffersDlgVisible = false;
    $scope.isCreateOfferDlgVisible = false;
    $scope.isEditOfferDlgVisible = false;
    $scope.isSelectCopyProjectDlgVisible = false;
    $scope.isCancelButtonHidden = false;
    $scope.isConfirmDlgVisible = false;
    $scope.isInputDlgVisible = false;
    $scope.isUploadAssetDlgVisible = false;
    $scope.uploadAssetFolder = null;
    $scope.areScrollsHidden = false;

    $scope.dialogMessage = '';
    $scope.dialogTitle = '';
    $scope.dialogCallbackOK = null;
    $scope.pageName = '';
    $scope.searchString = '';

    $scope.inputFormData = {};
    $scope.inputFormData.textData = '';
        
    $scope.openOfferDialogClick = function () {
        $scope.areScrollsHidden = true;
        $scope.isCreateOfferDlgVisible = true;
    };

    $scope.closeOfferDialogClick = function () {
        $scope.isCreateOfferDlgVisible = false;
        $scope.areScrollsHidden = false;
    };

    $scope.openEditOfferDialogClick = function (offerId) {
        $scope.areScrollsHidden = true;
        $scope.isEditOfferDlgVisible = true;

        $scope.$broadcast('editOfferDialog:opened', offerId);
    };

    $scope.closeEditOfferDialogClick = function () {
        $scope.isEditOfferDlgVisible = false;
        $scope.areScrollsHidden = false;
    };

    $scope.openCopyOfferDialogClick = function (offer) {
        $scope.areScrollsHidden = true;
        $scope.isSelectCopyProjectDlgVisible = true;

        //$scope.$broadcast('copyOfferDialog:opened', offerId);
        $scope.$broadcast('copyProjectDlgOpened', {
            dlgTitle: 'Copy content from another offer',
            dlgDescription: 'Select the project to copy the content.',
            onCancelClick: $scope.closeCopyOfferDialogClick,
            targetOfferId: offer.ID,
            targetOffer: offer
        });
    };

    $scope.closeCopyOfferDialogClick = function () {
        $scope.isSelectCopyProjectDlgVisible = false;
        $scope.areScrollsHidden = false;
    };

    $scope.openProjectDialogClick = function () {
        $scope.areScrollsHidden = true;

        $scope.isCreateProjectDlgVisible = true;
        $scope.$broadcast('projectDialog:opened');
    };

    $scope.closeProjectDialogClick = function () {
        $scope.isCreateProjectDlgVisible = false;
        $scope.areScrollsHidden = false;
    };

    $scope.openEditOfferstDialogClick = function () {
        $scope.areScrollsHidden = true;

        $scope.isEditOffersDlgVisible = true;
        $scope.$broadcast('editOffersDialog:opened');
    };

    $scope.closeEditOffersDialogClick = function () {
        $scope.isEditOffersDlgVisible = false;
        $scope.areScrollsHidden = false;
    };

    $rootScope.openConfimDialogClick = function (title, msg, callbackOK, btnOkText) {
        $scope.areScrollsHidden = true;

        $scope.dialogMessage = msg;
        $scope.dialogTitle = title ? title : "Confirm";
        $scope.btnOkText = btnOkText ? btnOkText : "Yes";
        $scope.isCancelButtonHidden = false;
        $scope.isConfirmDlgVisible = true;
        $scope.dialogCallbackOK = callbackOK;
    };

    $scope.openInformationDialogClick = function (title, msg, btnOkText) {
        $scope.dialogMessage = msg;
        $scope.dialogTitle = title ? title : "Information";
        $scope.btnOkText = btnOkText ? btnOkText : "Ok";
        $scope.isCancelButtonHidden = true;
        $scope.isConfirmDlgVisible = true;

        //Does not remove this line
        $scope.dialogCallbackOK = null;

        $scope.areScrollsHidden = true;
    };

    $scope.inputDialogCallbackOK = null;
    $scope.inputDialogMessage = "";
    $scope.inputDialogTitle = "";
    $scope.inputBtnOkText = "";

    $scope.openInputDialogClick = function (title, msg, callbackOK, btnOkText) {
        $scope.areScrollsHidden = true;

        $scope.inputDialogMessage = msg;
        $scope.inputDialogTitle = title ? title : "Confirm";
        $scope.inputBtnOkText = btnOkText ? btnOkText : "Confirm";
        $scope.inputFormData.textData = '';
        $scope.isInputDlgVisible = true;
        $scope.inputDialogCallbackOK = callbackOK;
    };

    $scope.uploadAssetDialogCallbackOK = null;
    $scope.uploadAssetDialogMessage = "";
    $scope.uploadAssetDialogTitle = "";
    $scope.uploadAssetBtnOkText = "";

    $scope.openUploadAssetDialogClick = function (title, msg, callbackOK, btnOkText, selectedFolder) {
        $scope.areScrollsHidden = true;
        $scope.uploadAssetDialogMessage = msg;
        $scope.uploadAssetDialogTitle = title ? title : "Confirm";
        $scope.uploadAssetBtnOkText = btnOkText ? btnOkText : "Confirm";
        $scope.isUploadAssetDlgVisible = true;
        $scope.uploadAssetDialogCallbackOK = callbackOK;
        $scope.uploadAssetFolder = selectedFolder;

        $scope.$broadcast('uploadAssetDlgOpened');
    };

    $scope.closeConfirmDialogClick = function () {
        $scope.isConfirmDlgVisible = false;
        $scope.areScrollsHidden = false;
    };

    $scope.closeInputDialogClick = function () {
        $scope.isInputDlgVisible = false;
        $scope.areScrollsHidden = false;
    };

    $scope.closeUploadAssetDialogClick = function () {
        $scope.isUploadAssetDlgVisible = false;
        $scope.areScrollsHidden = false;
    };

    $scope.confirmDialogOKClick = function () {
        $scope.closeConfirmDialogClick();

        if ($scope.dialogCallbackOK) {
            $scope.dialogCallbackOK();
        }
    };

    $scope.inputDialogOKClick = function () {
        if ($scope.inputFormData.textData != '') {
            $scope.closeInputDialogClick();

            if ($scope.inputDialogCallbackOK) {
                $scope.inputDialogCallbackOK($scope.inputFormData.textData);
            }
        }
    };

    $scope.uploadAssetDialogOKClick = function (asset) {
        $scope.closeUploadAssetDialogClick();

        if ($scope.uploadAssetDialogCallbackOK) {
            $scope.uploadAssetDialogCallbackOK(asset);
        }
    };

    $scope.showView = function (route) {
        $location.url('/' + route);
    };

    $scope.goto = function (url) {
        if (BASE_URL[BASE_URL.length - 1] !== '/')
            url = '/' + url;
        location.href = BASE_URL + url;
    }

    $scope.gotoPage = function (url, id) {
        if (BASE_URL[BASE_URL.length - 1] !== '/')
            url = '/' + url;
        location.href = BASE_URL + url + id;
    };

    $scope.loadDataForSharedMedia = function (name) {
        $scope.pageName = name;
    };

    $scope.showSearchResults = function () {
        if ($scope.searchString != undefined && $scope.searchString != '') {
            if (isAlphaNumeric($scope.searchString)) {
                $scope.goto('SearchResults/Index/#/?f=' + $scope.searchString);

                //Does not remove this line
                $scope.$broadcast('searchChanged', $scope.searchString);
            }
            else {
                $scope.openInformationDialogClick('Error', 'Only alphanumeric characters are allowed.');
            }
        }
    };

    $scope.createNewUser = function () {
        $scope.goto("Account/UserProfile/#/");
    }
});

