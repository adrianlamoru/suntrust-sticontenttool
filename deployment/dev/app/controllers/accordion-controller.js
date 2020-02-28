appControllers.controller('AccordionController', function ($scope) {
    $scope.activeSections = [];

    $scope.opened = function (id) {
        return $scope.activeSections.indexOf(id) > -1;
    };

    $scope.toggle = function (id) {
        var index = $scope.activeSections.indexOf(id);
        if (index > -1) {
            $scope.activeSections.splice(index, 1);
        } else {
            $scope.activeSections.push(id);
        }
    };

    $scope.onSelectedLayoutChange = function () {
        console.log('AccordionController ' + JSON.stringify($scope.selectedLayout));
    };
});