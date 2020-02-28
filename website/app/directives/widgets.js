
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
            pwmCallAction: "@",
            typeId: "@",
            agreementVisible: "@",
            addMobOptions: "@",
            addMobInAppLinkingOptions: "@",
            addPwmOptions: "@",
            addOlbOptions: "@",
            addOlbDeepLinksOptions: "@",
            addMobBrightfolio: "@",
            balanceTransferOpt: "@"
        },
        templateUrl: '../app/partials/call-to-action-component.html',
        controller: function ($scope, projectManager) {

            var CUSTOM_URL_VALUE = 'CustomURL';
            var defaultTypeId = $scope.typeId !== undefined ? Number($scope.typeId) : 2;
            
            $scope.isCustomURLSelected = $scope.pwmCallAction == "true" ? true : false;

            $scope.Options = [
                { Value: 'NULL', Name: 'Select Link Type' },
                { Value: 'NAC', Name: 'NAC' },
                { Value: 'gotoOLB-Statement', Name: 'OLB-Statement' },
                { Value: 'gotoOLB-BillPay', Name: 'OLB-BillPay' },
                { Value: 'goToOLBEmailAddress', Name: 'OLB-Email' },
                { Value: 'goToOLBMobileNumber', Name: 'OLB-Phone Number' }
            ];

            if ($scope.addMobBrightfolio) {
                $scope.Options.push({ Value: 'goToOLB-Brightfolio-MOB', Name: 'OLB-Brightfolio', Url: 'https://www1-itca.online-banking.suntrust.com/dfc/brightfolio' });
            } else {
                $scope.Options.push({ Value: 'goToOLB-Brightfolio', Name: 'OLB-Brightfolio', Url: 'https://www1-intg.online-banking.suntrust.com/dfc/brightfolio' });
            }

            $scope.Options.push({ Value: 'LearnMore', Name: 'Learn More (Detail Page)' });
            $scope.Options.push({ Value: CUSTOM_URL_VALUE, Name: 'Add Custom URL' });
            
            if ($scope.addMobOptions) {
                $scope.Options.push({ Value: 'gotoMOB-Alerts', Name: 'MOB-Alerts' });
                $scope.Options.push({ Value: 'gotoMOB-BillPay', Name: 'MOB-BillPay' });
                $scope.Options.push({ Value: 'gotoMOB-Statement', Name: 'MOB-Statement' });
            }

            if ($scope.addOlbOptions) {
                $scope.Options.push({ Value: 'goToOLB-ODP', Name: 'OLB-ODP' });
            }

            if ($scope.addOlbDeepLinksOptions) {
                $scope.Options.push({ Value: 'goToOLB-InternalTransfer', Name: 'OLB-Internal Transfer' });
                $scope.Options.push({ Value: 'goToOLB-ExternalTransfer', Name: 'OLB-External Transfer' });
                $scope.Options.push({ Value: 'goToOLB-OrderChecks', Name: 'OLB-Order Checks' });
                $scope.Options.push({ Value: 'goToOLB-ODC', Name: 'OLB-Overdraft Coverage' });
                $scope.Options.push({ Value: 'goToOLB-Alerts', Name: 'OLB-Alerts' });
                $scope.Options.push({ Value: 'goToOLB-EquityActivation', Name: 'OLB-Equity Activation' });
                $scope.Options.push({ Value: 'goToOLB-CreditLineIncrease', Name: 'OLB-Credit Line Increase' });
            }

            if ($scope.addMobInAppLinkingOptions) {
                $scope.Options.push({ Value: 'goToMOB-InternalTransfer', Name: 'MOB-Internal Transfer' });
                $scope.Options.push({ Value: 'goTo-Alerts', Name: 'Alerts' });
                $scope.Options.push({ Value: 'goToMOB-OverdraftProtection', Name: 'MOB-Overdraft Protection' });
                $scope.Options.push({ Value: 'goToMOB-BalanceTransfer', Name: 'MOB-Balance Transfer' });
                $scope.Options.push({ Value: 'goToMOB-EquityActivation', Name: 'MOB-Equity Activation' });
                $scope.Options.push({ Value: 'goToMOB-CheckDeposit', Name: 'MOB-Check Deposit' });
                $scope.Options.push({ Value: 'goToMOB-OrderChecks', Name: 'MOB-Order Checks' });
                $scope.Options.push({ Value: 'goToMOB-BillPayAddPayee', Name: 'MOB-Bill Pay Add Payee' });
                $scope.Options.push({ Value: 'goToMOB-CreditLineIncrease', Name: 'MOB-Credit Line Increase' });
            }

            if ($scope.balanceTransferOpt) {
                //$scope.Options.push({ Value: 'gotoOLB-BalanceTransfer', Name: 'OLB-Balance Transfer', Url: 'https://onlinebanking.suntrust.com/UI/accounts?dl=balancetransfer' });
                $scope.Options.push({ Value: 'gotoOLB-BalanceTransfer', Name: 'OLB-Balance Transfer', Url: '' });
            }

            if ($scope.addPwmOptions) {
                $scope.Options.push({ Value: 'goToWS-StatementDelivery', Name: 'OLB-Wealthscape Statement Delivery' });
            }

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
                if ($scope.pwmCallAction) {
                    $scope.component.Data.selectedOption = { Value: CUSTOM_URL_VALUE, Name: 'Add Custom URL' };
                } else {
                    initDropdowns($scope.component, $scope.Options);
                }
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


appDirectives.directive('wgEmailCallToAction', function () {
    return {
        restrict: "E",
        scope: {
            number: "@",
            goOnlineHeader: "@",
            goOnlineDetails: "@",
            callHeader: "@",
            callDetails: "@",
            visitHeader: "@",
            visitDetails: "@",
            deactivatable: "@",
            emailCtaType: "@"
        },
        templateUrl: '../app/partials/email-template/email-call-to-action-component.html',
        controller: function ($scope, projectManager) {
            
            $scope.init = function () {
                $scope.GoOnline = false;
                $scope.Call = false;
                $scope.Visit = false;
                
                $scope.component = { 'ID': 0, 'TypeID': 14, 'Data': { 'GoOnline': {}, 'Call': { }, 'Visit': { } } };
                $scope.component = projectManager.initComponent($scope.component);

                $scope.component.Data.GoOnline.Inactive = $scope.component.Data.GoOnline.Inactive ? $scope.component.Data.GoOnline.Inactive : false;
                $scope.component.Data.Call.Inactive = $scope.component.Data.Call.Inactive ? $scope.component.Data.Call.Inactive : false;
                $scope.component.Data.Visit.Inactive = $scope.component.Data.Visit.Inactive ? $scope.component.Data.Visit.Inactive : false;
                $scope.component.Data.NumberOfActiveCtas = $scope.component.Data.NumberOfActiveCtas ? $scope.component.Data.NumberOfActiveCtas : 0;
            };
            
            $scope.init();

            $scope.$on('project:reset', function () {
                $scope.init();
            });

            $scope.$watch('component.Data.NumberOfActiveCtas', function (newValue, oldValue) {
                
                switch (newValue) {
                    case '0':
                        $scope.component.Data.GoOnline.Inactive = true;
                        $scope.component.Data.Call.Inactive = true;
                        $scope.component.Data.Visit.Inactive = true;
                        break;
                    case '1':
                        $scope.component.Data.GoOnline.Inactive = false;
                        $scope.component.Data.Call.Inactive = true;
                        $scope.component.Data.Visit.Inactive = true;
                        break;
                    case '2':
                        $scope.component.Data.GoOnline.Inactive = false;
                        $scope.component.Data.Call.Inactive = false;
                        $scope.component.Data.Visit.Inactive = true;
                        break;
                    case '3':
                        $scope.component.Data.GoOnline.Inactive = false;
                        $scope.component.Data.Call.Inactive = false;
                        $scope.component.Data.Visit.Inactive = false;
                        break;
                }
            });
        }
    };
});

appDirectives.directive('wgEmailManageHeaderLinks', function () {
    return {
        restrict: "E",
        scope: {
            number: "@",
            firstLinkHeader: "@",
            firstLinkDetails: "@",
            secondLinkHeader: "@",
            secondLinkDetails: "@",
            thirdLinkHeader: "@",
            thirdLinkDetails: "@",
            deactivatable: "@",
            emailCtaType: "@"
        },
        templateUrl: '../app/partials/email-template/email-manage-header-links-component.html',
        controller: function ($scope, projectManager) {

            $scope.init = function () {
               
                $scope.component = { 'ID': 0, 'TypeID': 15, 'Data': { 'FirstLink': {}, 'SecondLink': {}, 'ThirdLink': {} } };
                $scope.component = projectManager.initComponent($scope.component);

                $scope.component.Data.FirstLink = $scope.component.Data.FirstLink ? $scope.component.Data.FirstLink : {};
                $scope.component.Data.SecondLink = $scope.component.Data.SecondLink ? $scope.component.Data.SecondLink : {};
                $scope.component.Data.ThirdLink = $scope.component.Data.ThirdLink ? $scope.component.Data.ThirdLink : {};

                $scope.component.Data.FirstLink.Inactive = $scope.component.Data.FirstLink.Inactive ? $scope.component.Data.FirstLink.Inactive : false;
                $scope.component.Data.SecondLink.Inactive = $scope.component.Data.SecondLink.Inactive ? $scope.component.Data.SecondLink.Inactive : false;
                $scope.component.Data.ThirdLink.Inactive = $scope.component.Data.ThirdLink.Inactive ? $scope.component.Data.ThirdLink.Inactive : false;
                $scope.component.Data.NumberOfHeaderLinks = $scope.component.Data.NumberOfHeaderLinks ? $scope.component.Data.NumberOfHeaderLinks : 0;
            };

            $scope.init();

            $scope.$on('project:reset', function () {
                $scope.init();
            });

            $scope.$watch('component.Data.NumberOfHeaderLinks', function (newValue, oldValue) {

                switch (newValue) {
                    case '0':
                        $scope.component.Data.FirstLink.Inactive = true;
                        $scope.component.Data.SecondLink.Inactive = true;
                        $scope.component.Data.ThirdLink.Inactive = true;
                        break;
                    case '1':
                        $scope.component.Data.FirstLink.Inactive = false;
                        $scope.component.Data.SecondLink.Inactive = true;
                        $scope.component.Data.ThirdLink.Inactive = true;
                        break;
                    case '2':
                        $scope.component.Data.FirstLink.Inactive = false;
                        $scope.component.Data.SecondLink.Inactive = false;
                        $scope.component.Data.ThirdLink.Inactive = true;
                        break;
                    case '3':
                        $scope.component.Data.FirstLink.Inactive = false;
                        $scope.component.Data.SecondLink.Inactive = false;
                        $scope.component.Data.ThirdLink.Inactive = false;
                        break;
                }
            });
        }
    };
});

appDirectives.directive('wgDetails', function () {
    return {
        restrict: "E",
        scope: {
            number: "@",
            name: "@",
            header: "@",
            details: "@",
            deactivatable: "@",
            emailTags: "@",
            specialControls: "@"
        },
        templateUrl: '../app/partials/details-component.html',
        controller: function ($rootScope, $scope, projectManager, $timeout, $route) {
            var commands = $scope.emailTags ? "insertEmailApplyCommand" : "insertCommand";
            if ($scope.specialControls) {
                //$scope.toolbarOptions = "[['h1', 'h2', 'h3', 'insertImage', 'fontColor'], ['p', 'bold', 'italics', 'underline', 'sup', 'sym', 'insertLink', 'ul', 'insertCommand', 'html']]";
                $scope.toolbarOptions = "[['fontSize', 'fontColor'], ['p', 'bold', 'italics', 'underline', 'sup', 'sym', 'insertLink', 'ul', '" + commands + "', 'html']]";
            } else {
                $scope.toolbarOptions = "[['p', 'bold', 'italics', 'underline', 'sup', 'sym', 'insertLink', 'ul', '" + commands + "', 'html']]";
            }

            $scope.component = { 'ID': 0, 'TypeID': 3, 'Inactive': false, 'Data': {} };
            $scope.ApplyPending = false;
            $scope.Options = [
                { Value: 'NULL', Name: 'Select desktop horizontal alignment' },
                { Value: 'left', Name: 'Left' },
                { Value: 'center', Name: 'Center' },
                { Value: 'right', Name: 'Right' },
                { Value: 'default', Name: 'Default' }
            ];

            $scope.init = function () {
                $scope.component = { 'ID': 0, 'TypeID': 3, 'Inactive': false, 'Data': {} };
                $scope.component = projectManager.initComponent($scope.component);
                $scope.componentName = $scope.name ? $scope.name : "Details";
               
                var proyectTypeID = projectManager.project.Layouts[0].LayoutDetail.OfferTypeID;

                if (proyectTypeID == 1) {
                    if (!$scope.component.Data.Apply || !$scope.component.Data.Apply.DesktopAlignment.Value) {
                        $scope.component.Data.Apply = {};
                        $scope.component.Data.Apply.DesktopAlignment = $scope.Options[0];
                        
                        
                    }

                    for (var i = 0; i < $scope.Options.length; i++) {
                        if ($scope.Options[i].Value == $scope.component.Data.Apply.DesktopAlignment.Value) {
                            $scope.component.Data.Apply.DesktopAlignment = $scope.Options[i];
                            break;
                        }
                    }
                    $scope.component.Data.Apply.width = $scope.component.Data.Apply.width ? $scope.component.Data.Apply.width : 100;
                }
            };

            $scope.init();

            $scope.$on('project:reset', function () {
                $scope.init();
            });

            $scope.$on('insertApplyBtn', function () {
                $scope.ApplyPending = true; 
                var horizontalPosition = $scope.component.Data.Apply ? $scope.component.Data.Apply.DesktopAlignment.Value : "NULL";
                $rootScope.horizontalPosition = horizontalPosition;
            });

           $scope.$watch('component.Data.Description', function (newValue, oldValue) {
                if (newValue !== oldValue) {

                    if ($scope.ApplyPending) {
                       var horizontalPosition = $scope.component.Data.Apply ? $scope.component.Data.Apply.DesktopAlignment.Value : "NULL";                        
                       $scope.component.Data.Description = $scope.component.Data.Description.replace(/apply-now-new/g, "apply-" + horizontalPosition);

                       var ctaWrapper = "<div class=\"apply-now\" style=\"display:inline-block;text-align:center;margin:10px 0;vertical-align:middle\">[[CTA-APPLY-NOW]]</div>";
                        if (horizontalPosition != "NULL" && horizontalPosition != "default") {
                            ctaWrapper = "<div class=\"apply-now\" style=\"margin:10px 0;text-align:" + horizontalPosition + ";display:block;\">[[CTA-APPLY-NOW]]</div>";
                        }
                    }
                }
            });


            $scope.$watch('component.Data.Apply.DesktopAlignment.Value', function (newValue, oldValue) {
               
                if (newValue !== oldValue) {
                    switch (oldValue) {
                        case "center":
                            $scope.component.Data.Description = $scope.component.Data.Description.replace(/apply-center/g, "apply-" + newValue)
                                .replace(/apply-now-new/g, "apply-" + newValue);
                            break;
                        case "left":
                            $scope.component.Data.Description = $scope.component.Data.Description.replace(/apply-left/g, "apply-" + newValue)
                                .replace(/apply-now-new/g, "apply-" + newValue);
                            break;
                        case "right":
                            $scope.component.Data.Description = $scope.component.Data.Description.replace(/apply-right/g, "apply-" + newValue)
                                .replace(/apply-now-new/g, "apply-" + newValue);
                            break;
                        case "default":
                            $scope.component.Data.Description = $scope.component.Data.Description.replace(/apply-default/g, "apply-" + newValue)
                                .replace(/apply-now-new/g, "apply-" + newValue);
                            break;
                        case "NULL":
                            $scope.component.Data.Description = $scope.component.Data.Description.replace(/apply-NULL/g, "apply-" + newValue)
                                .replace(/apply-now-new/g, "apply-" + newValue);
                            break;
                        default:
                            $scope.component.Data.Description = $scope.component.Data.Description.replace(/apply-now/g, "apply-" + newValue);
                            break
                    }
                }
            });

            $scope.$watch('component.Data.Apply.buttonText', function (newValue, oldValue) {

                $timeout(function () {
                    var applyNow = angular.element(document.querySelector("#testApply"));

                    if (applyNow[0]) {
                        $scope.component.Data.Apply.width = applyNow[0].clientWidth;
                    }
                }, 0);
                
            });

            $scope.reloadRoute = function () {
                $route.reload();
            }
        }
    };
});

appDirectives.directive('wgRightRail', function () {
    return {
        restrict: "E",
        scope: {
            number: "@",
            header: "@",
            details: "@",
            deactivatable: "@",
            specialControls: "@",
            openUploadImageDialogClick: "&",
            imageHeight: "@",
            imageWidth: "@",
            heightArray: "@",
            widthArray: "@",
            multiple: "@"
        },
        templateUrl: '../app/partials/email-template/right-rail-component.html',
        controller: function ($rootScope, $scope, FileUploader, projectManager, $timeout) {

            if ($scope.specialControls) {
                //$scope.toolbarOptions = "[['h1', 'h2', 'h3', 'insertImage', 'fontColor'], ['p', 'bold', 'italics', 'underline', 'sup', 'sym', 'insertLink', 'ul', 'insertCommand', 'html']]";
                $scope.toolbarOptions = "[['fontSize', 'fontColor'], ['p', 'bold', 'italics', 'underline', 'sup', 'sym', 'insertLink', 'ul', 'insertEmailCommand', 'html']]";
            } else {
                $scope.toolbarOptions = "[['p', 'bold', 'italics', 'underline', 'sup', 'sym', 'insertLink', 'ul', 'insertEmailCommand', 'html']]";
            }
            
            $scope.imageURL = '';
            $scope.uiMsg = '';
            var imageWidth = 0;
            var imageHeight = 0;
            var parentForm = $scope.$parent.sectionForm;
            var noContentHeader = angular.copy($scope.header);
            var noContentDetails = angular.copy($scope.details);

            $scope.Options = [
                { Value: 'NULL', Name: 'Select content type' },
                { Value: 'TextContent', Name: 'Text content' },
                { Value: 'ImageContent', Name: 'Image content' },
                { Value: 'CTAButtons', Name: 'CTA buttons' }
            ];
            
            var setSectionToDisplay = function () {
                $scope.showTextEditor = $scope.component.Data.selectedOption.Value === ContentType.TEXT_CONTENT;
                $scope.showImageSection = $scope.component.Data.selectedOption.Value === ContentType.IMAGE_CONTENT;
            }; 

            $scope.init = function () {
                $scope.component = { 'ID': 0, 'TypeID': 13, 'Inactive': false, 'Data': { 'Image': {}, 'TextEditor': {} } };
                $scope.component.Data.selectedOption = $scope.Options[0];
                $scope.component = projectManager.initComponent($scope.component);
                $scope.updateImageURL();
                $scope.uiMsg = '';
                setSectionToDisplay();
                initDropdowns($scope.component, $scope.Options);
            };

            $scope.$watch('component.Data.selectedOption', function (newValue, oldValue) {
                if (newValue !== oldValue) {
                    setSectionToDisplay();
                }
            });

            $scope.DEFAULT_URL = BASE_URL + "/Content/images/upload-image.png";

            var uploader = $scope.uploader = new FileUploader({
                url: BASE_URL + '/api/Media/Upload/' + projectManager.projectID
            });

            $scope.clickUploadImage = function () {
                $scope.openUploadImageDialogClick({ callbackOK: $scope.onImageUploaded });
            };

            $scope.onImageUploaded = function (data) {
                $scope.component.Data.Image.src = data.src;
                $scope.component.Data.Image.isExternal = data.isExternal;
                parentForm.$setDirty();
                $scope.updateImageURL();
            };

            $scope.updateImageURL = function () {
                if (!$scope.component.Data.Image.src) {
                    $scope.imageURL = $scope.DEFAULT_URL;
                } else {
                    if ($scope.component.Data.isExternal) {
                        $scope.imageURL = $scope.component.Data.Image.src;
                    } else {
                        $scope.imageURL = BASE_URL + $scope.component.Data.Image.src;
                    }
                }

                $scope.updateImageSize();
            };

            $scope.updateImageSize = function () {
                var image = new Image();

                image.onload = function () {
                    imageWidth = image.width;
                    imageHeight = image.height;
                    var heightErrorMsg = 'The image height should be: ' + $scope.imageHeight + 'px, exactly. Please provide the correct image size.';
                    var sizeErrorMsg = 'The image size should be: ' + $scope.imageWidth + 'px x ' + $scope.imageHeight + 'px, exactly. Please provide the correct image size.';
                    var errorMsg = $scope.imageWidth == 0 ? heightErrorMsg : sizeErrorMsg;

                    $scope.component.errors = [];
                    if ($scope.multiple) {
                        var imageWidthArray = $scope.widthArray.split(',');
                        var imageHeightArray = $scope.heightArray.split(',');
                        var index = 0;

                        var notValidDimensions = !imageWidthArray.includes(imageWidth.toString()) || !imageHeightArray.includes(imageHeight.toString());
                        var availableDimensions = "";

                        imageWidthArray.forEach(function (element) {
                            availableDimensions += element + 'px x ' + imageHeightArray[index++] + 'px, ';
                        });
                        errorMsg = 'The image size should be one of these availables: ' + availableDimensions + 'exactly. Please provide the correct image size.';
                    } else {
                        var notValidDimensions = $scope.imageWidth == 0 ? (imageHeight != $scope.imageHeight) : (imageWidth != $scope.imageWidth || imageHeight != $scope.imageHeight);
                    } 

                    if (notValidDimensions) {
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

            $scope.isImage = function (item) {
                var type = '|' + item.type.slice(item.type.lastIndexOf('/') + 1) + '|';
                return '|jpg|png|jpeg|bmp|gif|'.indexOf(type) !== -1;
            };

            // CALLBACKS

            uploader.onAfterAddingAll = function (addedFileItems) {

                if (addedFileItems.length > 1) {
                    $scope.uiMsg = 'Multiples files are not allowed.';
                    uploader.clearQueue();
                } else {
                    if (!$scope.isImage(addedFileItems[0].file)) {
                        $scope.uiMsg = 'The file is not a valid image type.';
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
                    $scope.uiMsg = 'Error uploading to server. Try again or select another file.';
                }

            };

            uploader.onCompleteItem = function (fileItem, response, status, headers) {
                if (uploader._isSuccessCode(status)) {
                    $scope.uiMsg = 'Upload completed!';

                    $scope.component.Data.Image.src = response[0];
                    $scope.component.Data.Image.isExternal = false;

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
            specialControls: "@",
            emailTags: "@",
            deactivatable: "@"
        },
        templateUrl: '../app/partials/disclaimer-component.html',
        controller: function ($scope, projectManager) {
            var commands = $scope.emailTags ? "insertEmailCommand" : "insertCommand";
            
            if ($scope.specialControls) {
                $scope.toolbarOptions = "[['fontSize', 'fontColor'], ['p', 'bold', 'italics', 'underline', 'sup', 'sym', 'insertLink', 'ul', '" + commands + "', 'html']]";
            } else {
                $scope.toolbarOptions = "[['p', 'bold', 'italics', 'underline', 'sup', 'sym', 'insertLink', 'ul', '" + commands + "', 'html']]";
            }
            
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
            addGhostOptions: "@",
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

            if ($scope.addGhostOptions) {
                $scope.Options.push({ Value: 'AdvantageChecking', Name: 'Advantage Checking' });
                $scope.Options.push({ Value: 'AdvantageCD', Name: 'Advantage CD' });
                $scope.Options.push({ Value: ' AdvantageMoneyMarketSavings', Name: ' Advantage Money Market Savings' });
            }

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
                    { Value: 'RemindMeLater', Name: 'Remind Me Later' }];

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
                { Value: 'NoThanks', Name: 'No, Thanks' }];

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
            name: "@",
            header: "@",
            details: "@",
            deactivatable: "@",
            openUploadImageDialogClick: "&",
            height: "@",
            width: "@",
            heightArray: "@",
            widthArray: "@",
            showTcTextEditor: "@",
            showLinkUrl: "@",
            multiple: "@"
        },
        templateUrl: '../app/partials/main-image-component.html',
        controller: function ($scope, FileUploader, projectManager, $timeout) {
            //$scope.toolbarOptions = "[['h1', 'h2', 'h3', 'insertImage', 'fontColor'], ['p', 'bold', 'italics', 'underline', 'sup', 'sym', 'insertLink', 'ul', 'insertCommand', 'html']]";
            $scope.toolbarOptions = "[['fontSize', 'fontColor'], ['p', 'bold', 'italics', 'underline', 'sup', 'sym', 'insertLink', 'ul', 'insertEmailCommand', 'html']]";
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
            $scope.Options = [
                { Value: 'NULL', Header: 'RobertoDkBlue_header', PiecePrefix: 'dkBlue', BaseColor: '#003B71', Name: 'Select Template' },
                { Value: 'darkBlue', Header: 'RobertoDkBlue_header', PiecePrefix: 'dkBlue', BaseColor:'#003B71', Name: 'Dark Blue Header' },
                { Value: 'lightBlue', Header: 'RobertoLtBlue_header', PiecePrefix: 'ltBlue', BaseColor: '#00A1E0', Name: 'Light Blue Header' },
                { Value: 'orange', Header: 'RobertoOrange_header', PiecePrefix: 'orange', BaseColor: '#EF7622', Name: 'Orange Header' },
                { Value: 'pink', Header: 'RobertoPink_header', PiecePrefix: 'pink', BaseColor: '#EB008A', Name: 'Pink Header' },
                { Value: 'teal', Header: 'RobertoTeal_header', PiecePrefix: 'teal', BaseColor: '#00B5AD', Name: 'Teal Header' }
            ];
            $scope.clickUploadImage = function () {
                $scope.openUploadImageDialogClick({ callbackOK: $scope.onImageUploaded });
            };

            $scope.onImageUploaded = function (data) {
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
                    var heightErrorMsg = 'The image height should be: ' + $scope.height + 'px, exactly. Please provide the correct image size.';
                    var sizeErrorMsg = 'The image size should be: ' + $scope.width + 'px x ' + $scope.height + 'px, exactly. Please provide the correct image size.';
                    var errorMsg =  $scope.width == 0 ? heightErrorMsg : sizeErrorMsg;

                    $scope.component.errors = [];
                    if ($scope.multiple) {
                        var imageWidthArray = $scope.widthArray.split(',');
                        var imageHeightArray = $scope.heightArray.split(',');
                        var index = 0;

                        var notValidDimensions = !imageWidthArray.includes(imageWidth.toString()) || !imageHeightArray.includes(imageHeight.toString());
                        var availableDimensions = "";
                        
                        imageWidthArray.forEach(function (element) {
                            availableDimensions += element + 'px x ' + imageHeightArray[index++] + 'px, ';
                        });
                        errorMsg = 'The image size should be one of these availables: ' + availableDimensions + 'exactly. Please provide the correct image size.';
                    } else {
                        var notValidDimensions = $scope.width == 0 ? (imageHeight != $scope.height) : (imageWidth != $scope.width || imageHeight != $scope.height);
                    }                   

                    if (notValidDimensions) {
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
                $scope.componentName = $scope.name ? $scope.name : "Main image";
                $scope.component.Data.HeaderColor = $scope.component.Data.HeaderColor ? $scope.component.Data.HeaderColor : 'rgb(255,255,255)';
                $(document).ready(function () {
                    $('#colorpicker').spectrum({
                        showPalette: true,
                        togglePaletteOnly: true,
                        showInput: true,
                        togglePaletteMoreText: 'more',
                        togglePaletteLessText: 'less',
                        color: $scope.component.Data.HeaderColor,
                        change: $scope.updateColor,
                        palette: [
                            ["#000", "#444", "#666", "#999", "#ccc", "#eee", "#f3f3f3", "#fff"],
                            ["#f00", "#f90", "#ff0", "#0f0", "#0ff", "#00f", "#90f", "#f0f"],
                            ["#f4cccc", "#fce5cd", "#fff2cc", "#d9ead3", "#d0e0e3", "#cfe2f3", "#d9d2e9", "#ead1dc"],
                            ["#ea9999", "#f9cb9c", "#ffe599", "#b6d7a8", "#a2c4c9", "#9fc5e8", "#b4a7d6", "#d5a6bd"],
                            ["#e06666", "#f6b26b", "#ffd966", "#93c47d", "#76a5af", "#6fa8dc", "#8e7cc3", "#c27ba0"],
                            ["#c00", "#e69138", "#f1c232", "#6aa84f", "#45818e", "#3d85c6", "#674ea7", "#a64d79"],
                            ["#900", "#b45f06", "#bf9000", "#38761d", "#134f5c", "#0b5394", "#351c75", "#741b47"],
                            ["#600", "#783f04", "#7f6000", "#274e13", "#0c343d", "#073763", "#20124d", "#4c1130"]
                        ]
                    });
                    
                });
                initDropdowns($scope.component, $scope.Options);
                $scope.component.Data.LearnMoreWidth = $scope.component.Data.LearnMoreWidth ? $scope.component.Data.LearnMoreWidth : 105;
            };

            $scope.updateColor = function () {
                $timeout(function () {
                    var colorPicker = angular.element('#colorpicker');
                    $scope.component.Data.HeaderColor = colorPicker[0].value ? colorPicker[0].value : 'rgb(255,255,255)';
                },0);
            };

            $scope.isImage = function (item) {
                var type = '|' + item.type.slice(item.type.lastIndexOf('/') + 1) + '|';
                return '|jpg|png|jpeg|bmp|gif|'.indexOf(type) !== -1;
            };

            // CALLBACKS

            uploader.onAfterAddingAll = function (addedFileItems) {

                if (addedFileItems.length > 1) {
                    $scope.uiMsg = 'Multiples files are not allowed.';
                    uploader.clearQueue();
                } else {
                    if (!$scope.isImage(addedFileItems[0].file)) {
                        $scope.uiMsg = 'The file is not a valid image type.';
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
            
            $scope.$watch('component.Data.LearnMoreText', function (newValue, oldValue) {

                $timeout(function () {
                    var learnMore = angular.element(document.querySelector("#testLearnMore"));
                    
                    if (learnMore[0]) {
                        $scope.component.Data.LearnMoreWidth = learnMore[0].clientWidth;
                    }
                }, 0);

            });
        }
    };
});

appDirectives.directive('wgSubHeadline', function () {
    return {
        restrict: "E",
        scope: {
            number: "@",
            header: "@",
            details: "@",
            deactivatable: "@"
        },
        templateUrl: '../app/partials/sub-headline-component.html',
        controller: function ($rootScope, $scope, projectManager) {
            $scope.toolbarOptions = "[['p', 'bold', 'italics', 'underline', 'sup', 'sym', 'insertLink', 'ul', 'insertCommand', 'html']]";
            $scope.component = { 'ID': 0, 'TypeID': 11, 'Inactive': false, 'Data': {} };

            $scope.init = function () {
                $scope.component = { 'ID': 0, 'TypeID': 11, 'Inactive': false, 'Data': {} };
                $scope.component = projectManager.initComponent($scope.component);
            };

            $scope.init();

            $scope.$on('project:reset', function () {
                $scope.init();
            });
        }
    };
});

appDirectives.directive('wgClickToChat', function () {
    return {
        restrict: "E",
        scope: {
            number: "@",
            header: "@",
            details: "@",
            deactivatable: "@",
            buttomTextVisible: "@",
            pwmCallAction: "@",
            typeId: "@"
        },
        templateUrl: '../app/partials/click-to-chat-component.html',
        controller: function ($scope, projectManager) {

            var CUSTOM_URL_VALUE = 'CustomURL';
            var DEFAULT_CLICK_TO_CHAT_TEXT = 'Chat now about this Offer';

            $scope.isCustomURLSelected = $scope.pwmCallAction == "true" ? true : false;

            $scope.Options = [
                { Value: 'goToChat', Name: 'Go-to-Chat' }
            ];

            $scope.component = { 'ID': 0, 'TypeID': 12, 'Inactive': false, 'Data': {} };
            $scope.component.Data.tcAgreement = false;
            

            $scope.clearCustomURL = function (item) {
                if ($scope.component.Data.selectedOption.Value != CUSTOM_URL_VALUE) {
                    $scope.component.Data.url = '';
                }
            };

            $scope.init = function () {
                $scope.component = { 'ID': 0, 'TypeID': 12, 'Inactive': false, 'Data': {} };
                $scope.component.Data.buttonText =  $scope.component.Data.buttonText ?  $scope.component.Data.buttonText : DEFAULT_CLICK_TO_CHAT_TEXT;
                $scope.component.Data.selectedOption = $scope.Options[0];

                $scope.component = projectManager.initComponent($scope.component);
                if ($scope.pwmCallAction) {
                    $scope.component.Data.selectedOption = { Value: CUSTOM_URL_VALUE, Name: 'Add Custom URL' };
                } else {
                    initDropdowns($scope.component, $scope.Options);
                }
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

appDirectives.directive('wgCcMainImage', function () {
    return {
        restrict: "E",
        scope: {
            number: "@",
            name: "@",
            header: "@",
            details: "@",
            deactivatable: "@",
            openUploadImageDialogClick: "&",
            height: "@",
            width: "@",
            heightArray: "@",
            widthArray: "@",
            showTcTextEditor: "@",
            showLinkUrl: "@",
            multiple: "@"
        },
        templateUrl: '../app/partials/main-image-component.html',
        controller: function ($scope, FileUploader, projectManager, $timeout) {
            //$scope.toolbarOptions = "[['h1', 'h2', 'h3', 'insertImage', 'fontColor'], ['p', 'bold', 'italics', 'underline', 'sup', 'sym', 'insertLink', 'ul', 'insertCommand', 'html']]";
            $scope.toolbarOptions = "[['fontSize', 'fontColor'], ['p', 'bold', 'italics', 'underline', 'sup', 'sym', 'insertLink', 'ul', 'insertEmailCommand', 'html']]";
            $scope.component = { 'ID': 0, 'TypeID': 16, 'Inactive': false, 'Data': {} };
            $scope.imageURL = '';
            $scope.uiMsg = '';
            var imageWidth = 0;
            var imageHeight = 0;
            var parentForm = $scope.$parent.sectionForm;
            
            $scope.DEFAULT_URL = BASE_URL + "/Content/images/upload-image.png";

            var uploader = $scope.uploader = new FileUploader({
                url: BASE_URL + '/api/Media/Upload/' + projectManager.projectID
            });
            $scope.Options = [
                { Value: 'NULL', Header: 'RobertoDkBlue_header', PiecePrefix: 'dkBlue', BaseColor: '#003B71', Name: 'Select Template' },
                { Value: 'darkBlue', Header: 'RobertoDkBlue_header', PiecePrefix: 'dkBlue', BaseColor:'#003B71', Name: 'Dark Blue Header' },
                { Value: 'lightBlue', Header: 'RobertoLtBlue_header', PiecePrefix: 'ltBlue', BaseColor: '#00A1E0', Name: 'Light Blue Header' },
                { Value: 'orange', Header: 'RobertoOrange_header', PiecePrefix: 'orange', BaseColor: '#EF7622', Name: 'Orange Header' },
                { Value: 'pink', Header: 'RobertoPink_header', PiecePrefix: 'pink', BaseColor: '#EB008A', Name: 'Pink Header' },
                { Value: 'teal', Header: 'RobertoTeal_header', PiecePrefix: 'teal', BaseColor: '#00B5AD', Name: 'Teal Header' }
            ];
            $scope.clickUploadImage = function () {
                $scope.openUploadImageDialogClick({ callbackOK: $scope.onImageUploaded });
            };

            $scope.onImageUploaded = function (data) {
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
                    var heightErrorMsg = 'The image height should be: ' + $scope.height + 'px, exactly. Please provide the correct image size.';
                    var sizeErrorMsg = 'The image size should be: ' + $scope.width + 'px x ' + $scope.height + 'px, exactly. Please provide the correct image size.';
                    var errorMsg =  $scope.width == 0 ? heightErrorMsg : sizeErrorMsg;

                    $scope.component.errors = [];
                    if ($scope.multiple) {
                        var imageWidthArray = $scope.widthArray.split(',');
                        var imageHeightArray = $scope.heightArray.split(',');
                        var index = 0;

                        var notValidDimensions = !imageWidthArray.includes(imageWidth.toString()) || !imageHeightArray.includes(imageHeight.toString());
                        var availableDimensions = "";
                        
                        imageWidthArray.forEach(function (element) {
                            availableDimensions += element + 'px x ' + imageHeightArray[index++] + 'px, ';
                        });
                        errorMsg = 'The image size should be one of these availables: ' + availableDimensions + 'exactly. Please provide the correct image size.';
                    } else {
                        var notValidDimensions = $scope.width == 0 ? (imageHeight != $scope.height) : (imageWidth != $scope.width || imageHeight != $scope.height);
                    }                   

                    if (notValidDimensions) {
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
                $scope.component = { 'ID': 0, 'TypeID': 16, 'Inactive': false, 'Data': {} };
                $scope.component = projectManager.initComponent($scope.component);
                $scope.updateImageURL();
                $scope.uiMsg = '';
                $scope.componentName = $scope.name ? $scope.name : "Main image";
                $scope.component.Data.HeaderColor = $scope.component.Data.HeaderColor ? $scope.component.Data.HeaderColor : 'rgb(255,255,255)';
                $(document).ready(function () {
                    $('#colorpicker').spectrum({
                        showPalette: true,
                        togglePaletteOnly: true,
                        showInput: true,
                        togglePaletteMoreText: 'more',
                        togglePaletteLessText: 'less',
                        color: $scope.component.Data.HeaderColor,
                        change: $scope.updateColor,
                        palette: [
                            ["#000", "#444", "#666", "#999", "#ccc", "#eee", "#f3f3f3", "#fff"],
                            ["#f00", "#f90", "#ff0", "#0f0", "#0ff", "#00f", "#90f", "#f0f"],
                            ["#f4cccc", "#fce5cd", "#fff2cc", "#d9ead3", "#d0e0e3", "#cfe2f3", "#d9d2e9", "#ead1dc"],
                            ["#ea9999", "#f9cb9c", "#ffe599", "#b6d7a8", "#a2c4c9", "#9fc5e8", "#b4a7d6", "#d5a6bd"],
                            ["#e06666", "#f6b26b", "#ffd966", "#93c47d", "#76a5af", "#6fa8dc", "#8e7cc3", "#c27ba0"],
                            ["#c00", "#e69138", "#f1c232", "#6aa84f", "#45818e", "#3d85c6", "#674ea7", "#a64d79"],
                            ["#900", "#b45f06", "#bf9000", "#38761d", "#134f5c", "#0b5394", "#351c75", "#741b47"],
                            ["#600", "#783f04", "#7f6000", "#274e13", "#0c343d", "#073763", "#20124d", "#4c1130"]
                        ]
                    });
                    
                });
                initDropdowns($scope.component, $scope.Options);
                $scope.component.Data.LearnMoreWidth = $scope.component.Data.LearnMoreWidth ? $scope.component.Data.LearnMoreWidth : 105;
            };

            $scope.updateColor = function () {
                $timeout(function () {
                    var colorPicker = angular.element('#colorpicker');
                    $scope.component.Data.HeaderColor = colorPicker[0].value ? colorPicker[0].value : 'rgb(255,255,255)';
                },0);
            };

            $scope.isImage = function (item) {
                var type = '|' + item.type.slice(item.type.lastIndexOf('/') + 1) + '|';
                return '|jpg|png|jpeg|bmp|gif|'.indexOf(type) !== -1;
            };

            // CALLBACKS

            uploader.onAfterAddingAll = function (addedFileItems) {

                if (addedFileItems.length > 1) {
                    $scope.uiMsg = 'Multiples files are not allowed.';
                    uploader.clearQueue();
                } else {
                    if (!$scope.isImage(addedFileItems[0].file)) {
                        $scope.uiMsg = 'The file is not a valid image type.';
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
            
            $scope.$watch('component.Data.LearnMoreText', function (newValue, oldValue) {

                $timeout(function () {
                    var learnMore = angular.element(document.querySelector("#testLearnMore"));
                    
                    if (learnMore[0]) {
                        $scope.component.Data.LearnMoreWidth = learnMore[0].clientWidth;
                    }
                }, 0);

            });
        }
    };
});