appControllers.controller('EditOfferController', function ($scope, $rootScope, CommonService) {
    var OfferModel = function () {
        this.ID = '';
        this.Name = '';
        this.ContentIDs = '';

        this.ContentIDtoAdd = '';
        this.ContentIDArray = [];
    }

    $scope.editOfferModel = new OfferModel();

    var init = function (offerId) {
        CommonService.getOfferForEdit(offerId,
               function (data) {
                   $scope.editOfferModel.ID = data.ID;
                   $scope.editOfferModel.Name = data.Name;
                   $scope.editOfferModel.ContentIDs = data.ContentIDs;
                   $scope.editOfferModel.Description = data.Description;
                   $scope.editOfferModel.OfferType = data.OfferType;
                   
                   var contentIDsArray = data.ContentIDs.split(",");

                   angular.forEach(contentIDsArray, function(contentID) {
                       if (contentID.trim() !== '') {
                           $scope.editOfferModel.ContentIDArray.push(contentID);
                       }
                   });
               },
               function (data, status) {
                   console.log("Error", data, status);
               });
    };

    $scope.$on('editOfferDialog:opened', function (e, offerId) {
        init(offerId);
    });

    $scope.isFormSubmitted = false;
    $scope.existingOfferIDErrorVisible = false;
    $scope.existingOfferIDErrorVisible = '';
    $scope.contentIDsErrorVisible = false;

    $scope.cancelButtonClick = function () {
        $scope.resetControls();

        $scope.closeEditOfferDialogClick();
    };

    $scope.resetControls = function () {
        $scope.isFormSubmitted = false;

        $scope.editOfferModel.ID = '';
        $scope.editOfferModel.Name = '';
        $scope.editOfferModel.Description = '';

        $scope.editOfferModel.ContentIDArray = [];
        $scope.editOfferModel.ContentIDtoAdd = '';

        $scope.contentIDsErrorVisible = false;
        $scope.contentIDsErrorText = '';
        $scope.existingOfferIDErrorVisible = false;
    };

    $scope.removeContentID = function (index) {
        $scope.editOfferModel.ContentIDArray.splice(index, 1);
    };

    $scope.addContentID = function () {
        if ($scope.editOfferModel.ContentIDtoAdd == '') {
            $scope.contentIDsErrorText = 'Content ID is required';
            $scope.contentIDsErrorVisible = true;
        } else if ($scope.editOfferModel.ContentIDArray.length >= 99) {
            $scope.contentIDsErrorText = 'The offer already have 99 IDs inserted';
            $scope.contentIDsErrorVisible = true;
        } else {
            try {
                for (var i = 0; i < $scope.editOfferModel.ContentIDArray.length; i++) {
                    if ($scope.editOfferModel.ContentIDArray[i] == $scope.editOfferModel.ContentIDtoAdd) {
                        $scope.contentIDsErrorText = 'This Content ID is already inserted';
                        $scope.contentIDsErrorVisible = true;
                        return;
                    }
                }

                $scope.editOfferModel.ContentIDArray.push($scope.editOfferModel.ContentIDtoAdd);

                $scope.editOfferModel.ContentIDtoAdd = '';
                $scope.contentIDsErrorVisible = false;
            }
            catch (e) {
                //Show in general errors
            }
        }
    };

    $scope.isCommitDisabled = function (form) {
        return form.offerName.$viewValue == undefined || form.offerName.$viewValue == '' ||
            ((form.offerContentID.$viewValue == undefined || form.offerContentID.$viewValue == '') && $scope.editOfferModel.ContentIDArray.length == 0);
    };

    $scope.isSuperAdmin = function () {
        if (CURRENT_ROL === "Super Admin") {
            return true;
        }

        return false;
    }

    $scope.saveOffer = function (form) {
        
        if (form.$valid) {
            if ($scope.editOfferModel.ContentIDArray.length == 0 && $scope.editOfferModel.ContentIDtoAdd == '') {
                $scope.contentIDsErrorText = 'At least one Content ID must be inserted';
                $scope.contentIDsErrorVisible = true;
            }
            else {
                $scope.contentIDsErrorVisible = '';

                if ($scope.editOfferModel.ContentIDtoAdd != '') {
                    $scope.addContentID();
                }

                if (!$scope.contentIDsErrorVisible) {
                    $scope.editOfferModel.ContentIDs = $scope.editOfferModel.ContentIDArray.join();

                    var offerId = $scope.editOfferModel.ID;

                    CommonService.saveOffer($scope.editOfferModel,
                        function () {
                            $scope.cancelButtonClick();
                            $rootScope.$broadcast('offerEdited', offerId);
                        },
                        function (data, status) {
                            $scope.$parent.openInformationDialogClick('Edit Offer', 'Unexpected error!. Status returned: ' + status);
                        });
                }
            }
        }
        else {
            $scope.isFormSubmitted = true;
        }
    };
});
