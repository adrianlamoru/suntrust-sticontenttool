
appDirectives.directive('draggable', function ($document) {
    return function (scope, element, attr) {
        var startX = 0, startY = 0, x = 0, y = 0;
        angular.element(document.querySelectorAll("#draggable")).on('mousedown', function (event) {
            startX = event.screenX - x;
            startY = event.screenY - y;
            $document.on('mousemove', mousemove);
            $document.on('mouseup', mouseup);
        });

        function mousemove(event) {
            y = event.screenY - startY;
            x = event.screenX - startX;
            element.css({
                top: y + 'px',
                left: x + 'px'
            });
        }

        function mouseup() {
            $document.off('mousemove', mousemove);
            $document.off('mouseup', mouseup);
        }
    };
});

appDirectives.directive('ngThumb', ['$window', function ($window) {
    var helper = {
        support: !!($window.FileReader && $window.CanvasRenderingContext2D),
        isFile: function(item) {
            return angular.isObject(item) && item instanceof $window.File;
        },
        isImage: function(file) {
            var type =  '|' + file.type.slice(file.type.lastIndexOf('/') + 1) + '|';
            return '|jpg|png|jpeg|bmp|gif|'.indexOf(type) !== -1;
        }
    };

    return {
        restrict: 'A',
        template: '<canvas/>',
        link: function(scope, element, attributes) {
            if (!helper.support) return;

            var params = scope.$eval(attributes.ngThumb);

            if (!helper.isFile(params.file)) return;
            if (!helper.isImage(params.file)) return;

            var canvas = element.find('canvas');
            var reader = new FileReader();

            reader.onload = onLoadFile;
            reader.readAsDataURL(params.file);

            function onLoadFile(event) {
                var img = new Image();
                img.onload = onLoadImage;
                img.src = event.target.result;
            }

            function onLoadImage() {
                var width = params.width || this.width / this.height * params.height;
                var height = params.height || this.height / this.width * params.width;
                canvas.attr({ width: width, height: height });
                canvas[0].getContext('2d').drawImage(this, 0, 0, width, height);
            }
        }
    };
}]);

appDirectives.directive('fallbackSrc', function () {
    return {
        link: function (scope, element, attributes) {
            element.bind('error', function () {
                element.attr('src', attributes.fallbackSrc)
            });
        }
    };
});

appDirectives.directive('integer', function () {
    return {
        require: 'ngModel',
        link: function (scope, elem, attr, ngModel) {

            function isInteger(value) {
                if (!isNumber(value) || parseInt(value) > INT_MAX_VALUE || parseInt(value) < 1) {
                    return false;
                }
                
                return true;
            }

            function validate(value) {
                var valid = isInteger(value);

                ngModel.$setValidity('integer', valid);

                return valid;
            }

            ngModel.$parsers.unshift(function (value) {
                ngModel.$setValidity('integer', isInteger(value));

                return value;
            });

            ngModel.$formatters.unshift(function (value) {
                ngModel.$setValidity('integer', isInteger(value));

                return value;
            });
        }
    }
});

appDirectives.directive('alphanumeric', function () {
    return {
        require: 'ngModel',
        restrict: 'A',
        link: function(scope, elem, attr, ngModel) {
 
            var validator = function(value) {
                if (/^[a-zA-Z0-9]*$/.test(value)) {
                    ngModel.$setValidity('alphanumeric', true);
                    return value;
                } else {
                    ngModel.$setValidity('alphanumeric', false);
                    return undefined;
                }
            };
            //For DOM -> model validation
            ngModel.$parsers.unshift(validator);
            //For model -> DOM validation
            ngModel.$formatters.unshift(validator);
        }
    };
});

appDirectives.directive('ngEnter', function() {
    return function(scope, element, attrs) {
        element.bind("keydown keypress", function(event) {
            if(event.which === 13) {
                scope.$apply(function(){
                    scope.$eval(attrs.ngEnter, {'event': event});
                });

                event.preventDefault();
            }
        });
    };
});
