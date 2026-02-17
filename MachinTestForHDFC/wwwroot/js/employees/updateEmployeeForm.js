$(document).ready(function () {

    //Get selected values
    var selectedDepartmentId = $("#DepartmentId").data('selected');
    var selectedCountryId = $("#CountryId").data('selected');
    var selectedStateId = $("#StateId").data('selected');
    var selectedCityId = $("#CityId").data('selected');

    //Load dropdown
    loadDepartmentDropdown(selectedDepartmentId);
    loadCountryDropdown(selectedCountryId, selectedStateId, selectedCityId);

    //Change function for load states
    $("#CountryId").change(function () {
        var countryId = $(this).val();

        $("#StateId").empty().append('<option value="">--Select State--</option>');
        $("#CityId").empty().append('<option value="">--Select City--</option>');

        if (countryId) {
            loadStateDropdown(countryId, null, null);
        }
    });

    //Change function for load cities
    $("#StateId").change(function () {
        var stateId = $(this).val();

        $("#CityId").empty().append('<option value="">--Select City--</option>');

        if (stateId) {
            loadCityDropdown(stateId, null);
        }
    });

    //Calculate tax deduction for salary
    $("#btnCalculateTax").click(function () {
        var salary = $("#Salary").val();

        if (salary <= 0) {
            alert("Enter valid salary.");
            return;
        }

        $.ajax({
            url: "/Employees/CalculateTax",
            type: "POST",
            data: { salary: salary },
            success: function (data) {
                $("#taxResult").text(data);
            },
            error: function () {
                alert("Error calculating tax.");
            }
        });
    });
});

//Function to load department dropdown
function loadDepartmentDropdown(selectedDepartmentId) {
    $.ajax({
        url: '/Department/GetAllDepartments',
        type: 'GET',
        success: function (data) {
            var dropDown = $("#DepartmentId");
            dropDown.empty();

            dropDown.append('<option value="">--Select Department--</option>');

            $.each(data, function (index, dept) {

                var isSelected = dept.id == selectedDepartmentId ? 'selected' : '';

                dropDown.append('<option value="' + dept.id + '" ' + isSelected + '> ' + dept.name + '</option > ');
            });
        },
        error: function (err) {
            console.log("Error loading departments: " + err);
        }
    });
}

//Function to load country dropdown
function loadCountryDropdown(selectedCountryId, selectedStateId, selectedCityId) {
    $.get('/CountryStateCity/GetAllCountries', function (data) {
        var dropdown = $("#CountryId");
        dropdown.empty();

        dropdown.append('<option value="">--Select Country--</option>');

        $.each(data, function (i, item) {
            dropdown.append(
                $('<option>', {
                    value: item.id,
                    text: item.countryName,
                    selected: item.id == selectedCountryId
                })
            );
        });

        if (selectedCountryId) {
            loadStateDropdown(selectedCountryId, selectedStateId, selectedCityId);
        }
    });
}

//Function to load state dropdown
function loadStateDropdown(selectedCountryId, selectedStateId, selectedCityId) {
    $.get('/CountryStateCity/GetAllStateByCountryId', { countryId: selectedCountryId }, function (data) {
        var dropdown = $("#StateId");
        dropdown.empty();

        dropdown.append('<option value="">--Select State--</option>');

        $.each(data, function (i, item) {
            dropdown.append(
                $('<option>', {
                    value: item.id,
                    text: item.stateName,
                    selected: item.id == selectedStateId
                })
            );
        });

        if (selectedStateId) {
            loadCityDropdown(selectedStateId, selectedCityId);
        }
    });
}

//Function to load city dropdown 
function loadCityDropdown(selectedStateId, selectedCityId) {
    $.get('/CountryStateCity/GetAllCityByStateId', { stateId: selectedStateId }, function (data) {
        var dropdown = $("#CityId");
        dropdown.empty();

        dropdown.append('<option value="">--Select City--</option>');

        $.each(data, function (i, item) {
            dropdown.append(
                $('<option>', {
                    value: item.id,
                    text: item.cityName,
                    selected: item.id == selectedCityId
                })
            );
        });
    });
}