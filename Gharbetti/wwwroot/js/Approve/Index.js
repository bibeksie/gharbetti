var app = angular.module('approveIndex', []);


app.controller('formController', ['$scope', '$filter', '$compile', '$http', '$rootScope', '$timeout', '$q', '$log', '$window',
    function ($scope, $filter, $compile, $http, $rootScope, $timeout, $q, $log, $window) {
        $scope.ApproveList = [];
        $scope.Address = {
            AddressLine1: "",
            AddressLine2: "",
            AddressLine3: "",
            City: "",
            PostalCode: "",
            County: "",
            Country: ""
        }


        $scope.init = function (tenantList) {

            $scope.ApproveList = tenantList;
        }

        $scope.onClickCloseModal = function () {
            $('#addModal').modal('hide');
        }

        $scope.onClickAddress = function (id) {

            $scope.Address = {
                AddressLine1: "",
                AddressLine2: "",
                AddressLine3: "",
                City: "",
                PostalCode: "",
                County: "",
                Country: ""
            }

            let data = $scope.ApproveList.find(x => x.Id == id);

            $scope.Address.AddressLine1 = data.AddressLine1;
            $scope.Address.AddressLine2 = data.AddressLine2;
            $scope.Address.AddressLine3 = data.AddressLine3;
            $scope.Address.PostalCode = data.PostalCode;
            $scope.Address.County = data.County;
            $scope.Address.Country = data.Country;
            $scope.Address.City = data.City;
            $('#addModal').modal('show');
        }

        $scope.onClickApprove = function (id) {

            if (confirm("Do you really want to Approve this Tenant Registration?")) {
                $http.get(`/api/Approve/register/?id=${id}`).then(function (responsedata) {
                    debugger;
                    console.log(responsedata.data);
                    if (responsedata.data.Status) {
                        location.reload();
                    }
                    else {
                        alert("Error Occured");
                    }
                });
            }
        }


    }]);









