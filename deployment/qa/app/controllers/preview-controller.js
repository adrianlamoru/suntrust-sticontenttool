appControllers.controller("PreviewController", function ($scope, $timeout) {
    this.$inject = ['$scope', '$timeout'];

    $scope.source = {};
    $scope.ComponentType = ComponentType;
    $scope.EmailCTAType = EmailCTAType;
    $scope.EmailHeaderLinks = EmailHeaderLinks;
    $scope.ContentType = ContentType;
    $scope.TemplatePiece = TemplatePiece;
    $scope.Device = Device;
    $scope.EmailTags = EmailTags;
    $scope.SectionType = SectionType;
    $scope.DEFAULT_IMAGE_URL = BASE_URL + "/Content/images/transparent.png";
    $scope.EMAIL_TEMPLATE_IMAGE_URL = BASE_URL + "/Content/images/email-template";
    $scope.PDF_IMAGE_URL = BASE_URL + "/Content/images/pdf-icon.png";

    $scope.bottomCtaCountClass = "";
    
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
            case 24:
                return BASE_URL + "/app/partials/preview/email-campaign/email-template.html";
            case 25:
                return BASE_URL + "/app/partials/preview/rto-all-offers.html";
        }
        return "";
    }; 

    $scope.renderSection = function () {
        if ($scope.source.selectedSection.ID) {
            updateCtasWrapperClass();
            return true;
        }
        return false;
    };

    $scope.isEmptyOrWhiteSpace = function(string) {
        if (string && string.trim() != '') {
            return false;
        }
        return true;
    }

    $scope.isVisible = function (typeID, elementType) {
        var component = $scope.getComponent(typeID);
        if (!component || component.Inactive)
            return false;

        switch (component.TypeID) {
            case $scope.ComponentType.CALL_TO_ACTION:
                if (!component.Inactive) {
                    if (component.Data.buttonText && component.Data.buttonText !== '' && component.Data.selectedOption.Value !== 'NULL')
                        return true;
                    else if ($scope.getHref(typeID) !== "") {
                        return true;
                    }
                    else if (component.Data.selectedOption && component.Data.selectedOption.Value !== '' && component.Data.selectedOption.Value !== 'NULL') {
                        return true;
                    }
                }
                return false;
            case $scope.ComponentType.CALL_TO_ACTION_2:
                if (!component.Inactive) {
                    if (component.Data.buttonText && component.Data.buttonText !== '' && component.Data.selectedOption.Value !== 'NULL')
                        return true;
                    else if ($scope.getHref(typeID) !== "") {
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
            case $scope.ComponentType.RIGHT_RAIL:
                    var visibleSection = !component.Inactive && component.Data.selectedOption && component.Data.selectedOption.Value !== 'NULL';
                    return visibleSection && $scope.isActive(typeID, component.Data.selectedOption.Value);
                break;
            case $scope.ComponentType.EMAIL_CALL_TO_ACTION:
                switch (elementType) {
                    case EmailCTAType.GO_ONLINE:
                        return !component.Data.GoOnline.Inactive && !$scope.isEmptyOrWhiteSpace(component.Data.GoOnline.url) && !$scope.isEmptyOrWhiteSpace(component.Data.GoOnline.buttonText);
                        break;
                    case EmailCTAType.CALL:
                        return !component.Data.Call.Inactive && !$scope.isEmptyOrWhiteSpace(component.Data.Call.url) && !$scope.isEmptyOrWhiteSpace(component.Data.Call.buttonText);
                        break;
                    case EmailCTAType.VISIT:
                        return !component.Data.Visit.Inactive && !$scope.isEmptyOrWhiteSpace(component.Data.Visit.url) && !$scope.isEmptyOrWhiteSpace(component.Data.Visit.buttonText);
                        break;
                }
                break;
            case $scope.ComponentType.EMAIL_HEADER_LINKS:
                switch (elementType) {
                    case EmailHeaderLinks.FIRST_LINK:
                        return !component.Data.FirstLink.Inactive && !$scope.isEmptyOrWhiteSpace(component.Data.FirstLink.url) && !$scope.isEmptyOrWhiteSpace(component.Data.FirstLink.text);
                        break;
                    case EmailHeaderLinks.SECOND_LINK:
                        return !component.Data.SecondLink.Inactive && !$scope.isEmptyOrWhiteSpace(component.Data.SecondLink.url) && !$scope.isEmptyOrWhiteSpace(component.Data.SecondLink.text);
                        break;
                    case EmailHeaderLinks.THIRD_LINK:
                        return !component.Data.ThirdLink.Inactive && !$scope.isEmptyOrWhiteSpace(component.Data.ThirdLink.url) && !$scope.isEmptyOrWhiteSpace(component.Data.ThirdLink.text);
                        break;
                }
                break;
        }

        return false;
    };

    $scope.isActive = function (typeID, contentType) {
        
        var component = $scope.getComponent(typeID);
        if (!component || (typeID === ComponentType.RIGHT_RAIL && !component.Data.selectedOption)) {
            return false;
        }
        
        switch (contentType) {
            case ContentType.CTA_BUTTONS:
                return (component.Data.selectedOption.Value === contentType) && ($scope.isVisible(ComponentType.EMAIL_CALL_TO_ACTION, EmailCTAType.GO_ONLINE) || $scope.isVisible(ComponentType.EMAIL_CALL_TO_ACTION, EmailCTAType.CALL) || $scope.isVisible(ComponentType.EMAIL_CALL_TO_ACTION, EmailCTAType.VISIT));
                break;
            case ContentType.TEXT_CONTENT:
                return component.Data.selectedOption.Value === contentType && !$scope.isEmptyOrWhiteSpace(component.Data.TextEditor.Description);
                break;
            case ContentType.IMAGE_CONTENT:
                return component.Data.selectedOption.Value === contentType && (!$scope.isEmptyOrWhiteSpace(component.Data.Image.src) || !$scope.isEmptyOrWhiteSpace(component.Data.Image.Description));
            case ContentType.HEADER_LINKS_CONTENT:
                return (component.Data.NumberOfHeaderLinks > 0) && ($scope.isVisible(ComponentType.EMAIL_HEADER_LINKS, EmailHeaderLinks.FIRST_LINK) || $scope.isVisible(ComponentType.EMAIL_HEADER_LINKS, EmailHeaderLinks.SECOND_LINK) || $scope.isVisible(ComponentType.EMAIL_HEADER_LINKS, EmailHeaderLinks.THIRD_LINK));
                break;
        }
    }

    $scope.getComponent = function (typeID) {
        return $scope.source.getComponent(typeID);
    };

    $scope.getSrc = function (typeID) {
        var DEFAULT = $scope.source.selectedSection.ID === $scope.SectionType.EMAIL_TEMPLATE ? "" : $scope.DEFAULT_IMAGE_URL;
        var component = $scope.getComponent(typeID);
        if (typeID === ComponentType.RIGHT_RAIL) {
            if (!component || !component.Data.Image.src) {
                return DEFAULT;
            }
        } else {
            if (!component || !component.Data.src) {
                return DEFAULT;
            }  
        }

        if (!component.Data.isExternal) {
            return BASE_URL + (typeID !== $scope.ComponentType.RIGHT_RAIL ? component.Data.src.replace(/\\/g, "/") : component.Data.Image.src.replace(/\\/g, "/"));
        } else {
            return typeID !== $scope.ComponentType.RIGHT_RAIL ? component.Data.src.replace(/\\/g, "/") : component.Data.Image.src.replace(/\\/g, "/");
        }
        
        return DEFAULT;
    };

    $scope.getTemplateImagePiece = function (typeID, piece, device) {
        var component = $scope.getComponent(typeID);
        if (typeID === ComponentType.MAIN_IMAGE) {
            
            switch (piece) {
                case "_header":
                    return $scope.EMAIL_TEMPLATE_IMAGE_URL + "/headers/" + component.Data.selectedOption.Header + device + ".jpg";
                    break;
                case "Arrow":
                    return $scope.EMAIL_TEMPLATE_IMAGE_URL + "/buttonPieces/" + component.Data.selectedOption.PiecePrefix + piece + ".gif";
                    break;
                case "Left":
                    return $scope.EMAIL_TEMPLATE_IMAGE_URL + "/buttonPieces/" + component.Data.selectedOption.PiecePrefix + piece + ".png";
                    break;
                case "Right":
                    return $scope.EMAIL_TEMPLATE_IMAGE_URL + "/buttonPieces/" + component.Data.selectedOption.PiecePrefix + piece + ".png";
                    break;
            }
        }
        
    }

    $scope.getTemplateBaseColor = function (typeID) {
        var component = $scope.getComponent(typeID);
        if (typeID === ComponentType.MAIN_IMAGE) {
            return component.Data.selectedOption.BaseColor;
        }
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

    $scope.isEfectiveLink = function (linkUrl) {
        if (!linkUrl || $scope.isEmptyOrWhiteSpace(linkUrl))
            return false;

        if (linkUrl.indexOf("http://") < 0 && linkUrl.indexOf("https://") < 0)
            return false;

        return true;
    }

    $scope.getHref = function (typeID) {
        var component = $scope.getComponent(typeID);
        if (!component)
            return false;

        if (component && component.Data.selectedOption) {
            if (component.Data.url && component.Data.selectedOption.Value === "CustomURL") {
                if (component.Data.url.indexOf("http://") < 0 && component.Data.url.indexOf("https://") < 0) {
                    return "http://" + component.Data.url;
                } else {
                    return component.Data.url;
                }
            } else if (component.Data.selectedOption.Value === "gotoOLB-BalanceTransfer") {

                if (component.Data.selectedOption.Url.indexOf("http://") < 0 && component.Data.selectedOption.Url.indexOf("https://") < 0) {
                    return "http://" + component.Data.selectedOption.Url;
                } else {
                    return component.Data.selectedOption.Url;
                }
            }

        }
        return "";
    };

    $scope.getHeaderLinkUrl = function (typeID, linkNumber) {
        var component = $scope.getComponent(typeID);
        if (!component)
            return;

        switch (linkNumber) {
            case EmailHeaderLinks.FIRST_LINK:
                if (!component.Data.FirstLink.url)
                    return;
                else if (!$scope.isEfectiveLink(component.Data.FirstLink.url)) {
                    return "http://" + component.Data.FirstLink.url;
                } else {
                    return component.Data.FirstLink.url;
                }
            case EmailHeaderLinks.SECOND_LINK:
                if (!component.Data.SecondLink.url)
                    return;
                else if (!$scope.isEfectiveLink(component.Data.SecondLink.url)) {
                    return "http://" + component.Data.SecondLink.url;
                } else {
                    return component.Data.SecondLink.url;
                }
            case EmailHeaderLinks.THIRD_LINK:
                if (!component.Data.ThirdLink.url)
                    return;
                else if (!$scope.isEfectiveLink(component.Data.ThirdLink.url)) {
                    return "http://" + component.Data.ThirdLink.url;
                } else {
                    return component.Data.ThirdLink.url;
                }
        }
    };
    
    $scope.getBackgroundColor = function (typeID) {
        var component = $scope.getComponent(typeID);
        var defaultBkColor = "#FFFFFF";

        if (!component)
            return defaultBkColor;

        var backgroundColor = $scope.isEmptyOrWhiteSpace(component.Data.HeaderColor) ? defaultBkColor : component.Data.HeaderColor;
        return backgroundColor;
    };
    
    $scope.useSolidColor = function (typeID) {
        var component = $scope.getComponent(typeID);
        
        if (!component)
            return true;
        
        return component.Data.UseSolidColor || !component.Data.src;
    };
    
    $scope.getLinkUrl = function (typeID) {
        var component = $scope.getComponent(typeID);
        if (!component || !component.Data.LinkUrl)
            return "";

        if (!$scope.isEfectiveLink(component.Data.LinkUrl)) {
            return "http://" + component.Data.LinkUrl;
        } else {
            return component.Data.LinkUrl;
        }
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
            case $scope.ComponentType.SUB_HEADLINE:
                return component.Data.Description;
                break;
        }

        return "";
    };

    $scope.displayHeaderCta = function (typeID) {
        var component = $scope.getComponent(typeID);
        if (!component)
            return false;
        return component.Data.DisplayHeaderCta;
    };
    
    $scope.getImageAltText = function (typeID) {
        var component = $scope.getComponent(typeID);
        if (!component)
            return;

        if (component.TypeID === ComponentType.RIGHT_RAIL) {
            return component.Data.Image.AltText;
        } else {
            return component.Data.AltText;
        }
    };
    
    $scope.getHeaderCtaText = function (typeID) {
        var component = $scope.getComponent(typeID);
        if (!component)
            return "Learn more";

        return $scope.isEmptyOrWhiteSpace(component.Data.LearnMoreText) ? "Learn more" : component.Data.LearnMoreText;
    };
    
    $scope.getHeaderLinkText = function (typeID, linkNumber) {
        var component = $scope.getComponent(typeID);
        if (!component)
            return "";

        switch (linkNumber) {
            case EmailHeaderLinks.FIRST_LINK:
                if (!component.Data.FirstLink.text)
                    return;
                return component.Data.FirstLink.text;

            case EmailHeaderLinks.SECOND_LINK:
                if (!component.Data.SecondLink.text)
                    return;
                return component.Data.SecondLink.text;

            case EmailHeaderLinks.THIRD_LINK:
                if (!component.Data.ThirdLink.text)
                    return;
                return component.Data.ThirdLink.text;
        }
    };
    
    $scope.getHeaderOverlayCopy = function (typeID) {
        var component = $scope.getComponent(typeID);
        if (!component)
            return "";

        return component.Data.HeaderOverlayCopy;
    };

    $scope.getHeaderLearnMoreUrl = function (typeID) {
        var component = $scope.getComponent(typeID);
        if (!component)
            return "";

        return component.Data.LearnMoreUrl;
    };

    $scope.getEmailCtaUrl = function (typeID, ctaType) {
        var component = $scope.getComponent(typeID);
        
        if (!component)
            return;

        if (component.TypeID === ComponentType.EMAIL_CALL_TO_ACTION) {
            
            switch (ctaType) {
                case EmailCTAType.GO_ONLINE:
                    if (!component.Data.GoOnline.url)
                        return;
                    else if (component.Data.GoOnline.url.indexOf("http://") < 0 && component.Data.GoOnline.url.indexOf("https://") < 0) {
                        return "http://" + component.Data.GoOnline.url;
                    } else {
                        return component.Data.GoOnline.url;
                    }
                case EmailCTAType.CALL:
                    if (!component.Data.Call.url)
                        return;
                    else if (component.Data.Call.url.indexOf("http://") < 0 && component.Data.Call.url.indexOf("https://") < 0) {
                        return "http://" + component.Data.Call.url;
                    } else {
                        return component.Data.Call.url;
                    }
                case EmailCTAType.VISIT:
                    if (!component.Data.Visit.url)
                        return;
                    else 
                    if (component.Data.Visit.url.indexOf("http://") < 0 && component.Data.Visit.url.indexOf("https://") < 0) {
                        return "http://" + component.Data.Visit.url;
                    } else {
                        return component.Data.Visit.url;
                    }
            }
        } 
    };

    $scope.getEmailButtonText = function (typeID, ctaType) {
        var component = $scope.getComponent(typeID);

        if (!component)
            return;

        if (component.TypeID === ComponentType.EMAIL_CALL_TO_ACTION) {

            switch (ctaType) {
                case EmailCTAType.GO_ONLINE:
                    return component.Data.GoOnline.buttonText;
                    
                case EmailCTAType.CALL:
                    return component.Data.Call.buttonText;
                    
                case EmailCTAType.VISIT:
                    return component.Data.Visit.buttonText;
                    
            }
        }
    };

    $scope.getEmailLinkText = function (typeID, ctaType) {
        var component = $scope.getComponent(typeID);

        if (!component)
            return;

        if (component.TypeID === ComponentType.EMAIL_CALL_TO_ACTION) {

            switch (ctaType) {
                case EmailCTAType.GO_ONLINE:
                    return $scope.isEmptyOrWhiteSpace(component.Data.GoOnline.LinkText) ? component.Data.GoOnline.url : component.Data.GoOnline.LinkText;
                    
                case EmailCTAType.CALL:
                    return $scope.isEmptyOrWhiteSpace(component.Data.Call.LinkText) ? component.Data.Call.url : component.Data.Call.LinkText;
                    
                case EmailCTAType.VISIT:
                    return $scope.isEmptyOrWhiteSpace(component.Data.Visit.LinkText) ? component.Data.Visit.url : component.Data.Visit.LinkText;
                    
            }
        }
    };

    $scope.$on('updateCtasWrapperClass', function (event) {
        updateCtasWrapperClass();
    });

    var updateCtasWrapperClass = function () {
        var activeItems = 0;
        activeItems = $scope.isVisible(ComponentType.EMAIL_CALL_TO_ACTION, EmailCTAType.GO_ONLINE) ? ++activeItems : activeItems;
        activeItems = $scope.isVisible(ComponentType.EMAIL_CALL_TO_ACTION, EmailCTAType.CALL) ? ++activeItems : activeItems;
        activeItems = $scope.isVisible(ComponentType.EMAIL_CALL_TO_ACTION, EmailCTAType.VISIT) ? ++activeItems : activeItems;
        
        switch (activeItems) {
            case 1:
                $scope.bottomCtaCountClass = "one-item";
                break;
            case 2:
                $scope.bottomCtaCountClass = "two-items";
                break;
            case 3:
                $scope.bottomCtaCountClass = "three-items";
                break;
            default:
                $scope.bottomCtaCountClass = "";
        }
    }

    $scope.translateEmailTags = function (textToTranslate, component) {
        var buttonText = "Apply Now";
        var buttonUrl = "";
        var horizontalPosition = "NULL";
        var buttonLeftImageUrl = $scope.getTemplateImagePiece(ComponentType.MAIN_IMAGE, TemplatePiece.LEFT, Device.DESKTOP);
        var buttonArrowImageUrl = $scope.getTemplateImagePiece(ComponentType.MAIN_IMAGE, TemplatePiece.ARROW, Device.DESKTOP);
        var buttonRightImageUrl = $scope.getTemplateImagePiece(ComponentType.MAIN_IMAGE, TemplatePiece.RIGHT, Device.DESKTOP);
        var buttonBackgroundColor = $scope.getTemplateBaseColor(ComponentType.MAIN_IMAGE);

        if (component) {
            buttonText = component.Data.Apply.buttonText ? component.Data.Apply.buttonText : buttonText;
            buttonUrl = component.Data.Apply.url;
            horizontalPosition = component.Data.Apply.DesktopAlignment.Value;
        }
        var cta = "<a target=\"_blank\" href=\"" + buttonUrl + "\" style=\"text-decoration:none;background-size:100% 100%;color: #FFFFFF; text-align: center; display: inline-block;\">" +
                    "<span style=\"height: 30px;width: 20px;display: inline-block;background-size: cover;background-image: url('" + buttonLeftImageUrl + "')\"></span>" +
                    "<span style=\"background-color:" + buttonBackgroundColor + "; color:#FFFFFF; height: 30px; padding: 0 20px;line-height: 30px; display: inline-block;vertical-align: top;\">" + buttonText + "</span>" +
                    "<span style=\"height: 30px;width: 10px;display: inline-block;background-size: cover;background-image: url('" + buttonArrowImageUrl + "')\"></span>" +
                    "<span style=\"height: 30px;width: 20px;display: inline-block;background-size: cover;background-image: url('" + buttonRightImageUrl + "')\"></span>" +
                  "</a>";
        //var cta = "<a target=\"_blank\" href=\"" + buttonUrl + "\" style=\"padding: 6px 35px;text-decoration:none;background-image: url('" + $scope.EMAIL_TEMPLATE_IMAGE_URL + "/apply-now-background.png'); background-size:100% 100%;color: #FFFFFF; text-align: center; display: inline-block\"><span>" + buttonText + "&nbsp;&nbsp;&nbsp;&nbsp; &#x25BA;</span></a>";
        //var cta = "<a target=\"_blank\" href=\"" + buttonUrl + "\" style=\"padding: 6px 35px;text-decoration:none;'); text-align: center; display: inline-block\">" +
        //    "<table>" +
        //        "<tr>" +
        //            "<td><img style=\"height: 32px;\" src=\"" + buttonLeftImageUrl + "\"/></td>" +
        //            "<td style=\"background-color:" + buttonBackgroundColor + "; color:#FFFFFF; line-height: 32px; padding: 0 20px;\"><span>" + buttonText + "</span></td>" +
        //            "<td><img style=\"height: 32px;\" src=\"" + buttonArrowImageUrl + "\"/></td>" +
        //            "<td><img style=\"height: 32px;\" src=\"" + buttonRightImageUrl + "\"/></td>" +
        //        "</tr>" +
        //    "</table>" +
        //    "</a>";

        if (!buttonUrl || (buttonUrl.indexOf("http://") < 0 && buttonUrl.indexOf("https://") < 0)) {
            if (textToTranslate) {
                textToTranslate = textToTranslate.replace(/display:block/g, "display:none")
                    .replace(/display:inline-block/g, "display:none")
                    .replace(/apply-now/g, "apply-now hidden");
            }
            cta = "";
        }

        if (!textToTranslate)
            return;
        //console.log("TRANSLATE ", textToTranslate);
        textToTranslate = textToTranslate.replace($scope.EmailTags.CASH_BACK_PERCENT.ID, $scope.EmailTags.CASH_BACK_PERCENT.Value)
            .replace($scope.EmailTags.CASH_BACK_MONTHS.ID, $scope.EmailTags.CASH_BACK_MONTHS.Value)
            .replace($scope.EmailTags.OTHER_CASH_BACK_PERCENT.ID, $scope.EmailTags.OTHER_CASH_BACK_PERCENT.Value)
            .replace($scope.EmailTags.INTRO_INTEREST.ID, $scope.EmailTags.INTRO_INTEREST.Value)
            .replace($scope.EmailTags.INTRO_INTEREST_MONTHS.ID, $scope.EmailTags.INTRO_INTEREST_MONTHS.Value)
            .replace($scope.EmailTags.PROMO_EXPIRE_DATE.ID, $scope.EmailTags.PROMO_EXPIRE_DATE.Value)
            .replace($scope.EmailTags.OFFER_AMOUNT.ID, $scope.EmailTags.OFFER_AMOUNT.Value)
            
            .replace(/\[\[CTA-APPLY-NOW\]\]/g, cta);

        var find = '<span class="apply-' + horizontalPosition + '"';
        var re = new RegExp(find, 'g');

        //textToTranslate = textToTranslate.replace(re, '<div class="apply-' + horizontalPosition + '"');
        
        return textToTranslate;
    };
    
    $scope.init = function (source) {
        if (source)
            $scope.source = source;
    };
});