appControllers.controller('CreateOfferController', function ($scope, $rootScope, CommonService) {
    
    var OfferModel = function () {
        this.ID = '';
        this.Name = '';
        this.Description = '';
        this.ContentIDs = '';

        this.ContentIDtoAdd = '';
        this.ContentIDArray = [];
    }
   
    $scope.isFormSubmitted = false;
    $scope.existingOfferIDErrorVisible = false;
    $scope.existingOfferIDErrorVisible = '';
    $scope.contentIDsErrorVisible = false;

    $scope.createOfferModel = new OfferModel();

    $scope.cancelButtonClick = function () {
        $scope.resetControls();

        $scope.closeOfferDialogClick();
    };

    $scope.resetControls = function () {
        $scope.isFormSubmitted = false;

        $scope.createOfferModel.ID = '';
        $scope.createOfferModel.Name = '';
        $scope.createOfferModel.Description = '';

        $scope.createOfferModel.ContentIDArray = [];
        $scope.createOfferModel.ContentIDtoAdd = '';

        $scope.contentIDsErrorVisible = false;
        $scope.contentIDsErrorText = '';
        $scope.existingOfferIDErrorVisible = false;
    };

    $scope.removeContentID = function (index) {
        $scope.createOfferModel.ContentIDArray.splice(index, 1);
    };

    $scope.offerTypes = [
        { Value: 0, Name: "Web Campaign" },
        { Value: 1, Name: "Email Campaign" }
    ];
    
    $scope.isAdmin = function () {
        if (CURRENT_ROL === "Administrator") {
            return true;
        }

        return false;
    }

    $scope.isSuperAdmin = function () {
        return CURRENT_ROL === "Super Admin";
    }

    $scope.addContentID = function () {
        if ($scope.createOfferModel.ContentIDtoAdd == '') {
            $scope.contentIDsErrorText = 'Content ID is required';
            $scope.contentIDsErrorVisible = true;
        } else if ($scope.createOfferModel.ContentIDArray.length >= 99) {
            $scope.contentIDsErrorText = 'The offer already have 99 IDs inserted';
            $scope.contentIDsErrorVisible = true;
        } else {
            try {
                for (var i = 0; i < $scope.createOfferModel.ContentIDArray.length; i++) {
                    if ($scope.createOfferModel.ContentIDArray[i] == $scope.createOfferModel.ContentIDtoAdd) {
                        $scope.contentIDsErrorText = 'This Content ID is already inserted';
                        $scope.contentIDsErrorVisible = true;
                        return;
                    }
                }

                $scope.createOfferModel.ContentIDArray.push($scope.createOfferModel.ContentIDtoAdd);

                $scope.createOfferModel.ContentIDtoAdd = '';
                $scope.contentIDsErrorVisible = false;
            }
            catch (e) {
                //Show in general errors
            }
        }
    };
    $scope.isCommitDisabled = function (form) {
        if ($scope.isSuperAdmin()) {
            return form.offerTypeSelect.$viewValue == undefined || form.offerTypeSelect.$viewValue == '' ||
                form.offerID.$viewValue == undefined || form.offerID.$viewValue == '' ||
                form.offerName.$viewValue == undefined || form.offerName.$viewValue == '' ||
                form.offerDescription.$viewValue == undefined || form.offerDescription.$viewValue == '' ||
                ((form.offerContentID.$viewValue == undefined || form.offerContentID.$viewValue == '') && $scope.createOfferModel.ContentIDArray.length == 0);
        } else {
            return form.offerID.$viewValue == undefined || form.offerID.$viewValue == '' ||
                form.offerName.$viewValue == undefined || form.offerName.$viewValue == '' ||
                form.offerDescription.$viewValue == undefined || form.offerDescription.$viewValue == '' ||
                ((form.offerContentID.$viewValue == undefined || form.offerContentID.$viewValue == '') && $scope.createOfferModel.ContentIDArray.length == 0);
        }
       
    };

    $scope.createOffer = function (form, createProject) {
        if (form.$valid) {

            if (!$scope.isSuperAdmin()) {
                $scope.selectedOfferType = { 'Value': CURRENT_USER_TYPE_ID};
            }
            if ($scope.createOfferModel.ID == "0") {
                $scope.$parent.openInformationDialogClick('Create Offer', 'Offer ID must be greater than 0.' + status);
            }
            else if ($scope.createOfferModel.ContentIDArray.length == 0 && $scope.createOfferModel.ContentIDtoAdd == '') {
                $scope.contentIDsErrorText = 'At least one Content ID must be inserted';
                $scope.contentIDsErrorVisible = true;
            }
            else {
                $scope.contentIDsErrorVisible = '';

                if ($scope.createOfferModel.ContentIDtoAdd != '') {
                    $scope.addContentID();
                }

                if (!$scope.contentIDsErrorVisible) {
                    $scope.createOfferModel.ContentIDs = $scope.createOfferModel.ContentIDArray.join();
                    $scope.createOfferModel.OfferType = $scope.selectedOfferType.Value;
                    
                    CommonService.createOffer($scope.createOfferModel,
                        function (offerCreated) {
                            if (createProject == true && offerCreated) {
                                //create the project, call to project layout page
                                var createProjectModel = new ProjectCreateEditViewModel();
                                createProjectModel.ID = offerCreated.ID;

                                CommonService.createProject(createProjectModel, function (data, status) {
                                    $scope.cancelButtonClick();

                                    if (data.ID) {
                                        $scope.$parent.gotoPage('Home/OfferProjectLayout#/layout-menu/', data.ID);
                                    } else {
                                        $scope.$parent.openInformationDialogClick(null, 'An error has occurred, please try again or contact support for assistance!.');
                                    }
                                });
                            }
                            else {
                                $scope.cancelButtonClick();
                                $rootScope.$broadcast('offerAdded');
                            }
                        },
                        function (data, status) {
                            if (status == 409) {
                                $scope.existingOfferIDErrorVisible = true;
                            }
                            else {
                                $scope.$parent.openInformationDialogClick('Create Offer', 'Unexpected error!. Status returned: ' + status);
                            }
                        });
                }
            }
        }

        else {
            $scope.isFormSubmitted = true;
        }
    };
});
