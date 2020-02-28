appControllers.controller('UserProfileController', function ($scope, $rootScope, $location, CommonService) {
    this.$inject = ['$scope', '$rootScope', '$location', 'CommonService'];

    $scope.viewMode = ViewMode.VIEW;
    $scope.location = $location;
    $scope.userId = 0;
    $scope.masterUser = { ID: -1, DateJoined : "", Email: "", Password: "", ConfirmPassword: "", FirstName: "", LastName: "", Role: -1 };
    $scope.user = angular.copy($scope.masterUser);

    $scope.loading = false;

    $scope.submitted = false;

    $scope.roles = [
        { Value: 0, Name: "Administrator" },
        { Value: 1, Name: "Project Manager" },
        { Value: 2, Name: "Editor" }
    ];

    $scope.selectedRole = null;

    $scope.enableFiledsForEdit = function () {
        if ($scope.viewMode &&
            $scope.viewMode == ViewMode.EDIT ||
            $scope.viewMode == ViewMode.CREATE) {
            return true;
        }
        return false;
    };

    $scope.updateViewMode = function (viewMode) {        
        $scope.viewMode = viewMode;
        if ($scope.viewMode == ViewMode.EDIT) {
            $scope.setupEditMode();
        }
    };

    $scope.passwordMatch = function () {
        if ($scope.user.Password === $scope.user.ConfirmPassword)
            return true;
        return false;
    }

    $scope.save = function () {
        if ($scope.userForm.$valid && $scope.passwordMatch()) {
            $scope.user.Role = $scope.selectedRole.Value;
            $scope.loading = true;
            CommonService.postUser($scope.user)
                .success(function (user) {
                    $scope.loading = false;
                    $scope.$parent.goto('Home/Dashboard');
                })
                .error(function (data, status) {
                    $scope.loading = false;
                    if (status == 409) {
                        $scope.$parent.openInformationDialogClick("Warning", 'This email address is already in use!');
                    } else {
                        console.log("Error", data, status);
                        $scope.$parent.openInformationDialogClick("Error", 'An error has occurred, please refresh the page or contact support for assistance!');
                    }

                });
        }
        $scope.submitted = true;
    }

    $scope.deleteUser = function () {
        $rootScope.openConfimDialogClick('Are You Sure?', 'Deleting a user will permanently remove them from the system. Are you sure you wish to continue?', function () {
            if ($scope.userId) {
                $scope.loading = true;
                CommonService.deleteUser($scope.userId)
                    .success(function (user) {
                        $scope.loading = false;
                        $scope.$parent.goto('Home/Dashboard');
                    })
                    .error(function (data, status) {
                        $scope.loading = false;
                        console.log("Error", data, status);
                        $scope.$parent.openInformationDialogClick("Error", 'An error has occurred, please refresh the page or contact support for assistance!');
                    });
            }
        }, 'CONFIRM');
    }

    $scope.cancel = function () {
        $scope.$parent.goto('Home/Dashboard');
        /*
        $scope.user = angular.copy($scope.masterUser);
        $scope.setupEditMode();
        $scope.userForm.$setPristine();
        */
    }

    $scope.loadUser = function () {
        if ($scope.userId) {
            $scope.loading = true;
            CommonService.getUser($scope.userId)
                .success(function (user) {
                    $scope.masterUser = user;
                    $scope.user = angular.copy($scope.masterUser);
                    $scope.setupEditMode();
                    $scope.userForm.$setPristine();
                    $scope.loading = false;
                })
                .error(function (data, status) {
                    $scope.loading = false;
                    if (status == 401) {
                        $scope.cancel();
                    } else {
                        console.log("Error", data, status);
                        $scope.$parent.openInformationDialogClick("Error", 'An error has occurred, please refresh the page or contact support for assistance!');
                    }
                });
        }        
    }

    $scope.init = function () {
        var viewMode = $scope.location.search().viewMode;
        var userId = $scope.location.search().userId;
        if (!userId) {
            $scope.viewMode = ViewMode.CREATE;
        } else {
            $scope.userId = userId;
            if (viewMode) {
                $scope.viewMode = viewMode;
            }
        }
        if ($scope.viewMode == ViewMode.VIEW || $scope.viewMode == ViewMode.EDIT) {
            $scope.loadUser();
        } else if ($scope.viewMode == ViewMode.CREATE && !($scope.isAdmin() || $scope.isSuperAdmin())) {
            $scope.cancel();
        }
    };

    $scope.isAdmin = function() {
        if (CURRENT_ROL === "Administrator") {
            return true;
        }

        return false;
    };

    $scope.isSuperAdmin = function () {
        if (CURRENT_ROL === "Super Admin") {
            return true;
        }

        return false;
    }

    $scope.isCurrentUser = function() {
        return $scope.userId == CURRENT_USER_ID;
    };

    $scope.setupEditMode = function() {
        $scope.selectedRole = $scope.getSelectedRole($scope.user.Role);
    };

    $scope.getSelectedRole = function(roleId) {
        for (var i = 0; i < $scope.roles.length; i++) {
            if ($scope.roles[i].Value == roleId) {
                return $scope.roles[i];
            }
        }

        return null;
    };

    $scope.init();
});