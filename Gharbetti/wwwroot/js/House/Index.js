var app = angular.module('houseIndex', ['ui.select', 'ngSanitize']);


app.controller('formController', ['$scope', '$filter', '$compile', '$http', '$rootScope', '$timeout', '$q', '$log', '$window',
    function ($scope, $filter, $compile, $http, $rootScope, $timeout, $q, $log, $window) {
        $scope.HouseList = [];
        $scope.RoomList = [];
        $scope.HouseModalTitle = "";
        $scope.MultipleRoom = [];

        $scope.House =
        {
            Id: 0,
            Name: "",
            Address: "",
            Street: "",
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

            $scope.HouseList = houseList;
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
                Street: "",
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

    }]);









