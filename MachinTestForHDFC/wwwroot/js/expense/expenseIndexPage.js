let currentPage = 1;
$(document).ready(function () {
    loadData();

    // SEARCH BUTTON CLICK
    $("#btnSearch").click(function () {

        currentPage = 1;
        loadData();
    });


    // PAGE SIZE CHANGE EVENT
    $("#pageSize").change(function () {

        currentPage = 1;
        loadData();
    });


    // ENTER KEY SEARCH
    $("#searchText").keypress(function (e) {

        if (e.which === 13) {
            $("#btnSearch").click();
        }
    });
});

function loadData() {
    let pageSize = $("#pageSize").val();
    let searchText = $("#searchText").val();

    $.ajax({
        url: `/ExpenseTransactions/GetExpenseTransactionData?searchText=${searchText}&pageNumber=${currentPage}&pageSize=${pageSize}`,
        type: 'GET',
        success: function (response) {
            bindTableData(response.data);
            bindPagination(response.totalPages);
        },
        error: function () {
            alert("Error loading data");
        }
    });
}

function bindTableData(data) {
    let rows = "";

    $.each(data, function (i, item) {
        let statusColor = item.status === "Approved" ? "green" : item.status === "Pending" ? "blue" : "red";

        rows += `
                <tr>
                <td>${item.employeeName}</td>
                <td>${item.categoryName}</td>
                <td>${formatDate(item.fromDate)}</td>
                <td>${formatDate(item.toDate)}</td>
                <td>${item.expenseAmount}</td>
                <td><span style="color:${statusColor}">${item.status}</span></td>
                <td>${item.description ?? ""}</td>
                <td>
                    <button class="btn btn-sm btn-outline-primary btnUpdateStatus"
                            data-id="${item.id}"
                            data-currentstatus="${item.status}">
                        Update Status
                    </button> |
                    <a href="/ExpenseTransactions/Edit/${item.id}"
                       class="btn btn-sm btn-outline-warning">
                       Edit
                    </a>
                    |
                    <a href="/ExpenseTransactions/Details/${item.id}"
                       class="btn btn-sm btn-outline-info">
                       Details
                    </a>
                    |
                    <a href="/ExpenseTransactions/Delete/${item.id}"
                       class="btn btn-sm btn-outline-danger">
                       Delete
                    </a>
                </td>
            </tr>        
        `;
    });

    $("#expenseTableBody").html(rows);
}

function bindPagination(totalPages) {
    let html = "";

    for (let i = 1; i <= totalPages; i++) {

        let active = (i === currentPage) ? "active" : "";

        html += `
            <li class="page-item ${active}">
                <a class="page-link" href="#" onclick="changePage(${i})">${i}</a>
            </li>
        `;
    }

    $("#pagination").html(html);
}

function changePage(page) {

    currentPage = page;
    loadData();
}

function formatDate(dateString) {

    if (!dateString) return "";

    let date = new Date(dateString);

    let day = ("0" + date.getDate()).slice(-2);
    let month = ("0" + (date.getMonth() + 1)).slice(-2);
    let year = date.getFullYear();

    return `${day}-${month}-${year}`;
}


$(document).on("click", ".btnUpdateStatus", function () {

    let id = $(this).data("id");
    let currentStatus = $(this).data("currentstatus");

    $("#expenseId").val(id);

    // Map text to value
    if (currentStatus === "Approved")
        $("#statusDropdown").val("A");
    else if (currentStatus === "Rejected")
        $("#statusDropdown").val("R");
    else
        $("#statusDropdown").val("P");

    $("#remark").val("");

    $("#statusModal").modal("show");
});

$("#btnSubmitStatus").click(function () {

    let id = $("#expenseId").val();
    let status = $("#statusDropdown").val();
    let remark = $("#remark").val();

    $.ajax({
        url: `/ExpenseTransactions/UpdateTransactionStatus?id=${id}&status=${status}&remark=${remark}`,
        type: 'POST',
        success: function (response) {

            alert(response);

            $("#statusModal").modal("hide");

            location.reload();
        },
        error: function () {
            alert("Error updating status");
        }
    });
});