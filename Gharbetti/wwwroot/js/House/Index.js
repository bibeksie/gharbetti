var app = angular.module('houseIndex', ['ui.select', 'ngSanitize', 'ui.bootstrap', 'ui.utils']);


app.controller('formController', ['$scope', '$filter', '$compile', '$http', '$rootScope', '$timeout', '$q', '$log', '$window',
    function ($scope, $filter, $compile, $http, $rootScope, $timeout, $q, $log, $window) {
        $scope.HouseList = {
            records: [],
        };
        $scope.RoomList = [];
        $scope.HouseModalTitle = "";
        $scope.MultipleRoom = [];
        $scope.StreetList = [];
        $scope.dataTableOpt = {
            "aLengthMenu": [[10, 50, 100, -1], [10, 50, 100, 'All']],
            "aoSearchCols": [
                null
            ],
        };
        var timeoutPromise;

        $scope.House =
        {
            Id: 0,
            Name: "",
            Address: "",
            Street: {
                selected: {}
            },
            SquareFootage: "",
            Remarks: "",
            RentAmount: 0,
            HouseRooms: [{

            }]
        };

        $scope.Multiple = {
            rooms: []
        }

        $scope.init = function (houseList) {

            $scope.HouseList.records = houseList;
            $scope.GetRoom();
        }

        $scope.onClickAdd = function () {

            if ($scope.House.Id == "") {
                data = $scope.House;
                data.HouseRoomViewModels = $scope.Multiple.rooms;

                $http.post("/api/House/Add", data).then(function (responsedata) {
                    debugger;
                    let result = responsedata.data;
                    if (result.Status) {
                        alert(result.Message);
                        location.reload();
                    }
                    else {
                        alert(result.Message);
                    }
                });
            }
            else {
                data = $scope.House;
                data.HouseRoomViewModels = $scope.House.HouseRooms;



                $http.post('/api/House/Edit', data).then(function (responsedata) {
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

        $scope.onClickAddModal = function () {
            $scope.HOuseModalTitle = "Add HOuse";

            $scope.House =
            {
                Id: 0,
                Name: "",
                Address: "",
                Street: {
                    selected: {}
                },
                SquareFootage: "",
                Remarks: "",
                RentAmount: 0,
                HouseRooms: [{

                }]
            };

            $('#addModal').modal('show');
        }

        $scope.onClickCloseModal = function () {
            $('#addModal').modal('hide');
        }

        $scope.ShowEdit = function (id) {
            $scope.HouseModalTitle = "Edit House";

            $http({
                url: "/api/House/Edit",
                method: "GET",
                params: { id: id }
            }).then(function (responsedata) {
                console.log(responsedata.data);
                if (responsedata.data) {
                    var data = responsedata.data.Data;
                    $scope.House = data;

                    if ($scope.House.HouseRoomViewModels) {
                        angular.forEach($scope.House.HouseRoomViewModels, function (value) {
                            let matchedRoom = $scope.RoomList.find(x => x.Id == value.RoomId);
                            if (matchedRoom) {
                                $scope.Multiple.rooms.push(matchedRoom);
                            }
                        });
                    }

                    $('#addModal').modal('show');
                }
                else {
                    alert(responsedata.data.Message);
                }
            });

        }

        $scope.Delete = function (id) {
            $http({
                url: "/api/House/Delete",
                method: "GET",
                params: { id: id }
            }).then(function (responsedata) {
                debugger;
                console.log(responsedata.data);
                if (responsedata.data) {
                    alert(responsedata.data.Message);
                    location.reload();
                }
                else {
                    alert(responsedata.data.Message);
                }
            });
        }

        $scope.GetRoom = function () {
            $http.get('/api/room').then(function (responsedata) {
                if (responsedata.data.Status) {
                    $scope.RoomList = responsedata.data.Data;
                }
            });
        }

        $scope.onLoadStreet = function () {

            $timeout.cancel(timeoutPromise);
            timeoutPromise = $timeout(function () {
                let data = {
                    postcode: $scope.House.PostalCode,
                    key: 'f22c4-5ef04-4b5b2-08e56',
                    response: 'data_formatted'
                }
                $scope.StreetList = [];
                let url = "//pcls1.craftyclicks.co.uk/json/rapidaddress";

                $http.post(url, data).then(function (responsedata) {
                    var result = responsedata.data;
                    if (result.error_code != "0002") {
                    $scope.StreetList = result.delivery_points;
                    }
                });
            }, 1000);
        }

    }]);









