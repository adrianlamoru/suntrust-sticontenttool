
appServices.service('projectManager', function () {
    this.project = {};
    this.projectID = 0;
    this.selectedLayout = {};
    this.selectedSection = {};

    this.newComponents = [];

    this.copyContentIDs = function (targetContentIDs) {
        for (var i = 0; i < targetContentIDs.length; i++) {
            var targetID = targetContentIDs[i];

            for (var j = 0; j < this.project.Layouts.length; j++) {
                if (targetID == this.project.Layouts[j].ID) {
                    var targetLayout = this.project.Layouts[j];

                    targetLayout.LayoutDetail.Note = this.selectedLayout.LayoutDetail.Note;
                    targetLayout.Sections = this.cloneObject(this.selectedLayout.Sections);
                }
            }
        }
    };

    this.setProject = function (project, sectionID) {
        this.project = project;

        if (this.project.Layouts) {
            if (!this.selectedLayout.ID && this.project.Layouts.length > 0) {
                this.selectedLayout = this.project.Layouts[0];
            } else {
                for (var i = 0; i < this.project.Layouts.length; i++) {
                    if (this.selectedLayout.ID === this.project.Layouts[i].ID) {
                        this.selectedLayout = this.project.Layouts[i];
                        break;
                    }
                }
            }
        }

        this.setSelectedSection(sectionID);
    };

    this.cloneObject = function (oldObject) {
        return JSON.parse(JSON.stringify(oldObject));
    };

    this.reset = function () {
        var sectionID = this.selectedSection.ID;
        var isNew = this.selectedSection.New;
        this.selectedSection = {};
        this.selectedSection.ID = sectionID;
        this.selectedSection.New = isNew;
        this.selectedSection.Components = [];
        this.newComponents = [];
    };

    this.EditMode = function () {
        return !this.selectedSection.New;
    };

    this.setSelectedSection = function (sectionID) {
        if (!sectionID || sectionID <= 0) {
            return;
        }

        if (this.selectedLayout
            && this.selectedLayout.Sections
            && this.selectedLayout.Sections.length > 0) {
            for (var i = 0; i < this.selectedLayout.Sections.length; i++) {
                var section = this.selectedLayout.Sections[i];
                if (section.ID === sectionID) {
                    this.selectedSection = this.cloneObject(section);
                    break;
                }
            }
        }

        if (!this.selectedSection.ID) {
            this.selectedSection.ID = sectionID;
            this.selectedSection.New = true;
            this.selectedSection.Components = [];
        }

        this.newComponents = [];
    };

    this.validateAndPrepareToSave = function () {
        var errors = [];

        for (var i = 0; i < this.newComponents.length; i++) {
            if (this.newComponents[i].isInvalid) {
                errors = errors.concat(this.newComponents[i].errors);
            }
        }

        for (var i = 0; i < this.selectedSection.Components.length; i++) {
            
            if (this.selectedSection.Components[i].isInvalid) {
                errors = errors.concat(this.selectedSection.Components[i].errors);
            }
        }

        if (errors.length > 0) {
            return errors;
        }

        if (this.newComponents.length > 0) {
            this.selectedSection.Components = this.selectedSection.Components.concat(this.newComponents);
            this.newComponents = [];
        }

        if (this.selectedSection.New) {
            this.selectedLayout.Sections.push(this.selectedSection);
        } else {
            for (var i = 0; i < this.selectedLayout.Sections.length; i++) {
                var section = this.selectedLayout.Sections[i];
                if (this.selectedSection.ID === this.selectedLayout.Sections[i].ID) {
                    this.selectedLayout.Sections[i] = this.selectedSection;
                    break;
                }
            }
        }

        return [];
    };

    this.isApproved = function () {
        return this.project.Approved;
    };

    this.setApproved = function (approved) {
        this.project.Approved = approved;
    };

    this.toggleApproval = function () {
        this.project.Approved = !this.project.Approved;
    };

    this.isThereAnySectionEdited = function () {
        return this.selectedLayout && this.selectedLayout.Sections && this.selectedLayout.Sections.length > 0;
    };

    this.isSectionEditable = function (sectionID) {
        if (this.selectedLayout && this.selectedLayout.Sections) {
            for (var i = 0; i < this.selectedLayout.Sections.length; i++) {
                if (this.selectedLayout.Sections[i].ID === sectionID) {
                    return true;
                }
            }
        }
        return false;
    };

    this.getComponent = function (typeID) {
        for (var i = 0; i < this.newComponents.length; i++) {
            if (this.newComponents[i].TypeID == typeID) {
                return this.newComponents[i];
            }
        }

        for (var i = 0; i < this.selectedSection.Components.length; i++) {
            if (this.selectedSection.Components[i].TypeID === typeID) {
                return this.selectedSection.Components[i];
            }
        }
    };

    this.initComponent = function (myComponent, reset) {
        if (!reset && this.selectedSection && this.selectedSection.Components) {
            for (var i = 0; i < this.selectedSection.Components.length; i++) {
                var component = this.selectedSection.Components[i];
                if (component.TypeID === myComponent.TypeID) {
                    myComponent = component;
                    break;
                }
            }
        }

        if (!myComponent.ID) {
            this.newComponents.push(myComponent);
        }

        return myComponent;
    };

    this.addComponent = function (newComponent) {
        console.log("addComponent");
        if (this.selectedSection && this.selectedSection.Components) {
            this.selectedSection.Components.push(newComponent);
            console.log("this.selectedSection: ", this.selectedSection, " component: ", newComponent);
        }
    };
});