
var initDropdowns = function (component, options) {
    if (component) {
        if (!component.Data.selectedOption || !component.Data.selectedOption.Value) {
            component.Data.selectedOption = options[0];
        }
        else {
            for (var i = 0; i < options.length; i++) {
                if (options[i].Value == component.Data.selectedOption.Value) {
                    component.Data.selectedOption = options[i];
                    break;
                }
            }
        }
    }
};

appDirectives.directive('wgCallToAction', function () {
    return {
        restrict: "E",
        scope: {
            number: "@",
            header: "@",
            details: "@",
            deactivatable: "@",
            buttomTextVisible: "@",
            typeId: "@",
            agreementVisible: "@"
        },
        templateUrl: '../app/partials/call-to-action-component.html',
        controller: function ($scope, projectManager) {
            var CUSTOM_URL_VALUE = 'CustomURL';
            var defaultTypeId = $scope.typeId !== undefined ? Number($scope.typeId) : 2;

            $scope.isCustomURLSelected = false;

            $scope.Options = [
                { Value: 'NULL', Name: 'Select Link Type' },
                { Value: 'NAC', Name: 'NAC' },
                { Value: 'gotoOLB-Statement', Name: 'OLB-Statement' },
                { Value: 'gotoOLB-BillPay', Name: 'OLB-BillPay' },
                { Value: 'goToOLBEmailAddress', Name: 'OLB-Email' },
                { Value: 'goToOLBMobileNumber', Name: 'OLB-Phone Number' },
                { Value: 'LearnMore', Name: 'Learn More (Detail Page)' },
                { Value: CUSTOM_URL_VALUE, Name: 'Add Custom URL' }];

            $scope.component = { 'ID': 0, 'TypeID': defaultTypeId, 'Inactive': false, 'Data': {} };
            $scope.component.Data.tcAgreement = false;

            $scope.clearCustomURL = function (item) {
                if ($scope.component.Data.selectedOption.Value != CUSTOM_URL_VALUE) {
                    $scope.component.Data.url = '';
                }
            };

            $scope.init = function () {
                $scope.component = { 'ID': 0, 'TypeID': defaultTypeId, 'Inactive': false, 'Data': {} };
                $scope.component = projectManager.initComponent($scope.component);

                initDropdowns($scope.component, $scope.Options);
            };

            $scope.$watch('component.Data.selectedOption', function (newValue, oldValue) {
                $scope.isCustomURLSelected = $scope.component.Data.selectedOption.Value == CUSTOM_URL_VALUE;
            });

            $scope.init();

            $scope.$on('project:reset', function () {
                $scope.init();
            });
        }
    };
});

appDirectives.directive('wgDetails', function () {
    return {
        restrict: "E",
        scope: {
            number: "@",
            header: "@",
            details: "@",
            deactivatable: "@"
        },
        templateUrl: '../app/partials/details-component.html',
        controller: function ($rootScope, $scope, projectManager) {

            //$scope.toolbarOptions = "[['h1', 'h2', 'h3', 'insertImage', 'fontColor'], ['p', 'bold', 'italics', 'underline', 'sup', 'sym', 'insertLink', 'ul', 'insertCommand', 'html']]";
            $scope.toolbarOptions = "[['p', 'bold', 'italics', 'underline', 'sup', 'sym', 'insertLink', 'ul', 'insertCommand', 'html']]";

            $scope.component = { 'ID': 0, 'TypeID': 3, 'Inactive': false, 'Data': {} };

            $scope.init = function () {
                $scope.component = { 'ID': 0, 'TypeID': 3, 'Inactive': false, 'Data': {} };
                $scope.component = projectManager.initComponent($scope.component);
            };

            $scope.init();

            $scope.$on('project:reset', function () {
                $scope.init();
            });
        }
    };
});

appDirectives.directive('wgBulletinDetails', function () {
    return {
        restrict: "E",
        scope: {
            number: "@",
            header: "@",
            details: "@",
            deactivatable: "@"
        },
        templateUrl: '../app/partials/offer-details-component.html',
        controller: function ($rootScope, $scope, projectManager) {

            //$scope.toolbarOptions = "[['h1', 'h2', 'h3', 'insertImage', 'fontColor'], ['p', 'bold', 'italics', 'underline', 'sup', 'sym', 'insertLink', 'ul', 'insertCommand', 'html']]";
            $scope.toolbarOptions = "[['p', 'bold', 'italics', 'underline', 'sup', 'sym', 'insertLink', 'ul', 'insertCommand', 'html']]";

            $scope.component = { 'ID': 0, 'TypeID': 9, 'Inactive': false, 'Data': {} };

            $scope.init = function () {
                $scope.component = { 'ID': 0, 'TypeID': 9, 'Inactive': false, 'Data': {} };
                $scope.component = projectManager.initComponent($scope.component);
            };

            $scope.init();

            $scope.$on('project:reset', function () {
                $scope.init();
            });
        }
    };
});

appDirectives.directive('wgTermsConditions', function () {
    return {
        restrict: "E",
        scope: {
            number: "@",
            header: "@",
            details: "@",
            deactivatable: "@",
            showTcTextEditor: "@",
            showTcUrl: "@"
        },
        templateUrl: '../app/partials/terms-conditions-component.html',
        controller: function ($scope, projectManager) {
            $scope.PDF_IMAGE_URL = BASE_URL + "/Content/images/pdficon_large.png";
            //$scope.toolbarOptions = "[['h1', 'h2', 'h3', 'insertImage', 'fontColor'], ['p', 'bold', 'italics', 'underline', 'sup', 'sym', 'insertLink', 'ul', 'insertCommand', 'html']]";
            $scope.toolbarOptions = "[['p', 'bold', 'italics', 'underline', 'sup', 'sym', 'insertLink', 'ul', 'insertCommand', 'html']]";
            $scope.component = { 'ID': 0, 'TypeID': 8, 'Inactive': false, 'Data': {} };

            $scope.init = function () {
                $scope.component = { 'ID': 0, 'TypeID': 8, 'Inactive': false, 'Data': {} };
                $scope.component = projectManager.initComponent($scope.component);
            };

            $scope.init();

            $scope.$on('project:reset', function () {
                $scope.init();
            });
        }
    };
});


appDirectives.directive('wgDisclaimer', function () {
    return {
        restrict: "E",
        scope: {
            number: '@',
            header: "@",
            details: "@",
            deactivatable: "@"
        },
        templateUrl: '../app/partials/disclaimer-component.html',
        controller: function ($scope, projectManager) {
            //$scope.toolbarOptions = "[['h1', 'h2', 'h3', 'insertImage', 'fontColor'], ['p', 'bold', 'italics', 'underline', 'sup', 'sym', 'insertLink', 'ul', 'insertCommand', 'html']]";
            $scope.toolbarOptions = "[['p', 'bold', 'italics', 'underline', 'sup', 'sym', 'insertLink', 'ul', 'insertCommand', 'html']]";
            $scope.component = { 'ID': 0, 'TypeID': 4, 'Inactive': false, 'Data': {} };

            $scope.init = function () {
                $scope.component = { 'ID': 0, 'TypeID': 4, 'Inactive': false, 'Data': {} };
                $scope.component = projectManager.initComponent($scope.component);
            };

            $scope.init();

            $scope.$on('project:reset', function () {
                $scope.init();
            });
        }
    }
});

appDirectives.directive('wgHeadline', function () {
    return {
        restrict: "E",
        scope: {
            maxLength: '@',
            number: '@',
            header: "@",
            details: "@",
            showPullDown: "@",
            deactivatable: "@"
        },
        templateUrl: '../app/partials/headline-component.html',
        controller: function ($scope, projectManager) {
            //$scope.toolbarOptions = "[['p', 'bold', 'italics', 'underline', 'sup', 'sym', 'insertLink', 'ul', 'insertCommand', 'html']]";

            $scope.Options = [
                    { Value: 'Checking', Name: 'Checking' },
                    { Value: 'Savings', Name: 'Savings' },
                    { Value: 'MoneyMarket', Name: 'Money Market' },
                    { Value: 'SignatureAdvantage', Name: 'Signature Advantage' },
                    { Value: 'SignatureAdvantageWithBrokerage', Name: 'Signature Advantage with Brokerage' },
                    { Value: 'CD', Name: 'CD' },
                    { Value: 'IRA', Name: 'IRA' },
                    { Value: 'InstallmentLoan', Name: 'Installment Loan' },
                    { Value: 'MortgageLoan', Name: 'Mortgage Loan' },
                    { Value: 'LineOfCredit', Name: 'Line of Credit' },
                    { Value: 'Access3', Name: 'Access 3' },
                    { Value: 'CreditCard', Name: 'Credit Card' },
            ];

            $scope.component = { 'ID': 0, 'TypeID': 7, 'Inactive': false, 'Data': {} };

            $scope.init = function () {
                $scope.component = { 'ID': 0, 'TypeID': 7, 'Inactive': false, 'Data': {} };
                $scope.component = projectManager.initComponent($scope.component);

                initDropdowns($scope.component, $scope.Options);
            };

            $scope.init();

            $scope.$on('project:reset', function () {
                $scope.init();
            });
        }
    }
});

appDirectives.directive('wgReminder', function () {
    return {
        restrict: "E",
        scope: {
            number: '@',
            header: "@",
            details: "@",
            deactivatable: "@"
        },
        templateUrl: '../app/partials/reminder-component.html',
        controller: function ($scope, projectManager) {
            $scope.Options = [
                    { Value: 'NULL', Name: 'Select an Option' },
                    { Value: 'RemindMeLater', Name: 'Remind me later' }];

            $scope.component = { 'ID': 0, 'TypeID': 5, 'Inactive': false, 'Data': {} };
            $scope.init = function () {
                $scope.component = { 'ID': 0, 'TypeID': 5, 'Inactive': false, 'Data': {} };
                $scope.component = projectManager.initComponent($scope.component);

                initDropdowns($scope.component, $scope.Options);
            };

            $scope.init();

            $scope.$on('project:reset', function () {
                $scope.init();
            });
        }
    }
});

appDirectives.directive('wgOfferRejection', function () {
    return {
        restrict: "E",
        scope: {
            number: '@',
            header: "@",
            details: "@",
            deactivatable: "@"
        },
        templateUrl: '../app/partials/offer-rejection-component.html',
        controller: function ($scope, projectManager) {
            $scope.Options = [
                { Value: 'NULL', Name: 'Select an Option' },
                { Value: 'NoThanks', Name: 'No, thanks' }];

            $scope.component = {
                'ID': 0, 'TypeID': 6, 'Inactive': false, 'Data': {},
            };

            $scope.component.Data.selectedOption = $scope.Options[0];

            $scope.init = function () {
                $scope.component = { 'ID': 0, 'TypeID': 6, 'Inactive': false, 'Data': {} };
                $scope.component = projectManager.initComponent($scope.component);

                initDropdowns($scope.component, $scope.Options);
            };

            $scope.init();

            $scope.$on('project:reset', function () {
                $scope.init();
            });
        }
    }
});

appDirectives.directive('wgMainImage', function () {
    return {
        restrict: "E",
        scope: {
            number: "@",
            header: "@",
            details: "@",
            deactivatable: "@",
            openUploadImageDialogClick: "&",
            height: "@",
            width: "@",
            showTcTextEditor: "@"
        },
        templateUrl: '../app/partials/main-image-component.html',
        controller: function ($scope, FileUploader, projectManager) {
            //$scope.toolbarOptions = "[['h1', 'h2', 'h3', 'insertImage', 'fontColor'], ['p', 'bold', 'italics', 'underline', 'sup', 'sym', 'insertLink', 'ul', 'insertCommand', 'html']]";
            $scope.toolbarOptions = "[['p', 'bold', 'italics', 'underline', 'sup', 'sym', 'insertLink', 'ul', 'insertCommand', 'html']]";
            $scope.component = { 'ID': 0, 'TypeID': 1, 'Inactive': false, 'Data': {} };
            $scope.imageURL = '';
            $scope.uiMsg = '';
            var imageWidth = 0;
            var imageHeight = 0;
            var parentForm = $scope.$parent.sectionForm;

            $scope.DEFAULT_URL = BASE_URL + "/Content/images/upload-image.png";

            var uploader = $scope.uploader = new FileUploader({
                url: BASE_URL + '/api/Media/Upload/' + projectManager.projectID
            });

            $scope.clickUploadImage = function () {
                $scope.openUploadImageDialogClick({ callbackOK: $scope.onImageUploaded });
            };

            $scope.onImageUploaded = function (data) {
                //console.log('Results from Upload Dialog ' + JSON.stringify(data));
                $scope.component.Data.src = data.src;
                $scope.component.Data.isExternal = data.isExternal;

                parentForm.$setDirty();

                $scope.updateImageURL();
            };

            $scope.updateImageURL = function () {
                if (!$scope.component.Data.src) {
                    $scope.imageURL = $scope.DEFAULT_URL;
                } else {
                    if ($scope.component.Data.isExternal) {
                        $scope.imageURL = $scope.component.Data.src;
                    } else {
                        $scope.imageURL = BASE_URL + $scope.component.Data.src;
                    }
                }

                $scope.updateImageSize();
            };

            $scope.updateImageSize = function () {
                var image = new Image();

                image.onload = function () {
                    imageWidth = image.width;
                    imageHeight = image.height;
                    var errorMsg = 'The image size should be: ' + $scope.width + 'px x ' + $scope.height + 'px, exactly. Please provide the correct image size.';

                    $scope.component.errors = [];

                    if (imageWidth != $scope.width || imageHeight != $scope.height) {
                        if (image.src != $scope.DEFAULT_URL) {
                            $scope.component.isInvalid = true;
                            $scope.component.errors.push(errorMsg);
                        } else {
                            errorMsg = "";
                        }

                        $scope.uiMsg = errorMsg;
                    }
                    else {
                        $scope.component.isInvalid = false;

                        errorMsg = '';
                    }

                    $scope.$apply(function () {
                        $scope.uiMsg = errorMsg;
                    });
                };

                image.src = $scope.imageURL;
            };

            $scope.init = function () {
                $scope.component = { 'ID': 0, 'TypeID': 1, 'Inactive': false, 'Data': {} };
                $scope.component = projectManager.initComponent($scope.component);
                $scope.updateImageURL();
                $scope.uiMsg = '';
            };

            $scope.isImage = function (item) {
                var type = '|' + item.type.slice(item.type.lastIndexOf('/') + 1) + '|';
                return '|jpg|png|jpeg|bmp|gif|'.indexOf(type) !== -1;
            };

            // CALLBACKS

            uploader.onAfterAddingAll = function (addedFileItems) {
                //console.info('onAfterAddingAll', addedFileItems);

                if (addedFileItems.length > 1) {
                    $scope.uiMsg = 'Multiples files are not allowed.';
                    uploader.clearQueue();
                } else {
                    if (!$scope.isImage(addedFileItems[0].file)) {
                        $scope.uiMsg = 'The file is not a valid image type.'
                        uploader.clearQueue();
                    } else {
                        //TODO: Confirmation Dialog
                        $scope.uiMsg = 'Uploading to server ...';

                        addedFileItems[0].upload();
                    }
                }
            };

            uploader.onProgressItem = function (fileItem, progress) {
                $scope.uiMsg = 'Uploading to server: ' + progress + '%';
            };

            uploader.onErrorItem = function (fileItem, response, status, headers) {
                if (status == 400) {
                    $scope.uiMsg = 'Error: ' + response.Message;
                }
                else {
                    console.info('onErrorItem', fileItem, response, status, headers);
                    $scope.uiMsg = 'Error uploading to server. Try again or select another file.';
                }

            };

            uploader.onCompleteItem = function (fileItem, response, status, headers) {
                if (uploader._isSuccessCode(status)) {
                    $scope.uiMsg = 'Upload completed!';

                    $scope.component.Data.src = response[0];
                    $scope.component.Data.isExternal = false;

                    parentForm.$setDirty();

                    $scope.updateImageURL();
                }
            };

            $scope.init();

            $scope.$on('project:reset', function () {
                $scope.init();
            });
        }
    };
});