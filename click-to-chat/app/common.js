var INT_MAX_VALUE = 2147483647;

var ComponentType = {
    MAIN_IMAGE: 1,
    CALL_TO_ACTION: 2,
    DETAILS: 3,
    DISCLAIMER: 4,
    REMINDER: 5,
    OFFER_REJECTION: 6,
    HEADLINE: 7,
    TERMS_CONDITIONS: 8,
    OFFER_DETAILS: 9,
    CALL_TO_ACTION_2: 10,
    CLICK_TO_CHAT: 12
};

var SectionType = {
    PRIMARY_BANNER: 1,
    DETAILS_PAGE: 2,
    RECOMMENDED_ACCOUNT: 3,
    ALL_OFFERS: 4,
    SPLASH_PAGE: 5,
    SIGN_OFF_PAGE: 6,
    CREDIT_CARDS: 7,
    DEPOSITS: 8,
    EQUITY: 9,
    CD: 10,
    BULLETIN_ZONE: 11,
    PWM_HERO_OFFR: 12,
    PWM_EDU_OFFR: 13,
    PWM_SPECIAL_OFFR: 14,

    PWM_BULLETIN: 15,
    PMW_LEARN_MORE: 16,
    PRIMARY_OFFR: 17,
    VIEW_ALL: 18,
    PWM_BULLETIN_ZONE: 19,
    GHOST_OFFR: 20,
    LEARN_MORE: 21,
    RTO_PRIMARY_OFFR: 22,
    RTO_LEARN_MORE: 23
};

var ViewMode = {
    CREATE: "Create",
    VIEW: "View",
    EDIT: "Edit"
};


//TODO: Add all events used in the appa
var events = {
    saveAndExportForReview: "saveAndExportForReview"
};

var IMAGE_SUBTYPE_VALUE = 'IMAGE';

var IMAGES_FILE_TYPE_FILTER = "IMAGE";
var PDF_FILE_TYPE_FILTER = "PDF";
var TEXTS_FILE_TYPE_FILTER = "TEXT";
var SPREADSHEETS_FILE_TYPE_FILTER = "SPREADSHEET";
var PRESENTATIONS_FILE_TYPE_FILTER = "PRESENTATION";
var OTHERS_FILE_TYPE_FILTER = "OTHER";

function PreviewViewModel(project, layoutID, sectionID) {
    var self = this;
    self.project = project;
    self.ComponentType = ComponentType;
    self.layoutID = layoutID;
    self.sectionID = sectionID;
    self.selectedLayout = {};
    self.selectedSection = {};

    self.init = function () {
        self.setSelectedLayout(self.layoutID);
        self.setSelectedSection(self.sectionID);
    };

    self.setSelectedLayout = function (layoutID) {
        for (var i = 0; i < self.project.Layouts.length; i++) {
            if (self.project.Layouts[i].ID === layoutID) {
                self.selectedLayout = self.project.Layouts[i];
                break;
            }
        }
    };

    self.setSelectedSection = function (sectionID) {
        for (var i = 0; self.selectedLayout && i < self.selectedLayout.Sections.length; i++) {
            if (self.selectedLayout.Sections[i].ID === sectionID) {
                self.selectedSection = self.selectedLayout.Sections[i];
                break;
            }
        }
    }; 

    self.getComponent = function (typeID) {
       
        for (var i = 0; self.selectedSection.Components && (i < self.selectedSection.Components.length) ; i++) {
            if (self.selectedSection.Components[i].TypeID == typeID) {
                return self.selectedSection.Components[i];
            }
        }
    };

    self.init();
}

function TableModel(data, orderBy, itemsPerPage) {
    var self = this;

    self.sortingOrder = 'default';
    self.reverse = false;
    self.itemsPerPage = itemsPerPage ? itemsPerPage : 10;
    self.currentPage = 0;
    self.pagedItems = [];
    self.pagedItemsLength = 0;
    self.data = data;
    self.orderBy = orderBy;
    self.selectedItem = null;

    self.setSelectedItem = function (item) {
        if (item) {
            self.selectedItem = item;
        } else {
            self.selectedItem = null;
            /*
            if (self.pagedItems.length > 0 && self.pagedItems[self.currentPage].length > 0) {
                self.selectedItem = self.pagedItems[self.currentPage][0];
            } */
        }
    };

    self.toggleSelectAll = function () {
        var nothing = self.nothingIsSelected();

        if (self.data) {
            self.data.forEach(function (item) {
                item.selected = nothing;
            });
        }
    };

    self.nothingIsSelected = function () {
        if (!self.data)
            return true;

        for (var i = 0; i < self.data.length; i++) {
            var contentID = self.data[i];
            if (contentID.selected) {
                return false;
            }
        }

        return true;
    }

    self.selectAllBtnDisplay = function () {
        return self.nothingIsSelected() ? "Select All" : "Deselect All";
    };

    self.isNotEmpty = function () {
        return self.data && self.data.length > 0;
    };

    self.unselectAll = function () {
        if (self.data) {
            self.data.forEach(function (item) {
                item.selected = false;
            });
        }
    };

    self.getSelectedItems = function () {
        var selectedItems = [];

        if (self.data) {
            self.data.forEach(function (item) {
                if (item.selected) {
                    selectedItems.push(item);
                }
            });
        }

        return selectedItems;
    };

    self.groupToPages = function () {
        if (self.data) {
            for (var i = 0; i < self.data.length; i++) {
                if (i % self.itemsPerPage === 0) {
                    self.pagedItems[Math.floor(i / self.itemsPerPage)] = [self.data[i]];
                } else {
                    self.pagedItems[Math.floor(i / self.itemsPerPage)].push(self.data[i]);
                }
            }
            self.pagedItemsLength = self.pagedItems.length;
        }
    };

    self.pageNumber = function () {
        if (self.pagedItemsLength === 0) {
            return 0;
        }

        return self.currentPage + 1;
    };

    self.previousDisabled = function () {
        if (self.currentPage == 0) {
            return true;
        }
    };

    self.nextDisabled = function () {
        if (self.pagedItemsLength == 0 || self.currentPage == self.pagedItemsLength - 1) {
            return true;
        }

        return false;
    };
    
    self.range = function (start, end) {
        var ret = [];
        if (!end) {
            end = start;
            start = 0;
        }
        for (var i = start; i < end; i++) {
            ret.push(i);
        }
        return ret;
    };

    self.prevPage = function () {
        if (self.currentPage > 0) {
            self.currentPage--;
            self.setSelectedItem();
        }
    };

    self.nextPage = function () {
        if (self.currentPage < self.pagedItemsLength - 1) {
            self.currentPage++;
            self.setSelectedItem();
        }
    };

    self.firstPage = function () {
        self.currentPage = 0;
        self.setSelectedItem();
    };

    self.lastPage = function () {
        self.currentPage = self.pagedItemsLength - 1;
        self.setSelectedItem();
    };

    self.setPage = function () {
        self.currentPage = self.n;
        self.setSelectedItem();
    };

    self.sortBy = function (newSortingOrder, reverse) {

        if (!reverse && self.sortingOrder == newSortingOrder) {
            self.reverse = !self.reverse;
        } else {
            self.reverse = reverse;
        }

        self.sortingOrder = newSortingOrder;

        if (self.orderBy) {
            self.data = self.orderBy(self.data, self.sortingOrder, self.reverse);
        }
        
        self.groupToPages();

        self.firstPage();
    };

    self.groupToPages();

    return self;
};

function post(path, params, method) {
    method = method || "post";
    var form = document.createElement("form");
    form.setAttribute("method", method);
    form.setAttribute("action", path);

    for (var key in params) {
        if (params.hasOwnProperty(key)) {
            var hiddenField = document.createElement("input");
            hiddenField.setAttribute("type", "hidden");
            hiddenField.setAttribute("name", key);
            hiddenField.setAttribute("value", params[key]);

            form.appendChild(hiddenField);
        }
    }

    document.body.appendChild(form);
    form.submit();
}

function isNumber(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}

function isAlphaNumeric(value) {
    if (/[^a-zA-Z0-9\s\-]/.test(value)) {
        return false;
    }

    return true;
}

var getImagePreview = function (fileItem) {
    if (fileItem.subtype == IMAGE_SUBTYPE_VALUE) {
        return BASE_URL + fileItem.src;
    }
    else if (fileItem.subtype == PDF_FILE_TYPE_FILTER) {
        return BASE_URL + '/Content/Images/pdf.jpg';
    }
    else if (fileItem.subtype == TEXTS_FILE_TYPE_FILTER) {
        return BASE_URL + '/Content/Images/doc.jpg';
    }
    else if (fileItem.subtype == SPREADSHEETS_FILE_TYPE_FILTER) {
        return BASE_URL + '/Content/Images/xls.jpg';
    }
    else if (fileItem.subtype == PRESENTATIONS_FILE_TYPE_FILTER) {
        return BASE_URL + '/Content/Images/ppt.jpg';
    }

    return BASE_URL + '/Content/Images/other.jpg';
};

var ProjectCreateEditViewModel = function () {
    this.ID = 0;
    this.CreatedBy = 0;
    this.Approved = false;
};

var hasInvalidCharacters = function (filename) {
    var nameWithoutExt = filename.substr(0, filename.lastIndexOf('.')) || filename;
    if (nameWithoutExt.match(/[ ~#%&\*{}\\:\<\>?/\+|".]+/i)) {
        return true;
    }

    return false;
};

