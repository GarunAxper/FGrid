﻿var table;

$(document).ready(function () {
    $.fn.dataTable.moment("DD/MM/YYYY HH:mm:ss");
    $.fn.dataTable.moment("DD/MM/YYYY");
    let columns = [];

    $(".dataTable thead tr th").each(function () {
        let columnName = $(this).text().replace(/\s/g, '');
        if (columnName !== undefined && columnName !== "") {
            columnName = columnName[0].toLowerCase() + columnName.substring(1, columnName.length);

            let index = columnName.indexOf(".");

            if (index !== -1) {
                columnName = columnName.substring(0, index + 1)
                    + columnName[index + 1].toLowerCase()
                    + columnName.substring(index + 2);
            }

            columns.push({data: columnName});
        }
    });

    $('.dataTable thead tr').clone(true).appendTo('.dataTable thead');
    $('.dataTable thead tr:eq(1) th').not('.not-searchable').each(function (i) {
        var title = $(this).text();
        // placeholder="Search '+title.replace(/\s/g, '')+'" /
        $(this).html('<input type="text" class="form-control form-control-sm">');

        $('input', this).on('keyup change', function () {
            if (table.column(i).search() !== this.value) {
                table
                    .column(i)
                    .search(this.value)
                    .draw();
            }
        });
    });

    table = $(".dataTable").DataTable({
        // Design Assets
        stateSave: true,
        orderCellsTop: true,
        fixedHeader: true,

        scrollX: true,
        autoWidth: true,
        // ServerSide Setups
        processing: true,
        serverSide: true,
        // Paging Setups
        paging: true,
        // Searching Setups
        searching: {regex: true},
        // Ajax Filter
        ajax: {
            url: "Api/Users/GetUsers",
            type: "POST",
            contentType: "application/json",
            dataType: "json",
            data: function (d) {
                return JSON.stringify(d);
            }
        },
        // Columns Setups
        columns: columns,
        // Column Definitions
        columnDefs: [
            {targets: "not-orderable", orderable: false},
            {targets: "not-searchable", searchable: false},
            {
                targets: "trim",
                render: function (data, type, full, meta) {
                    if (type === "display") {
                        data = strtrunc(data, 10);
                    }

                    return data;
                }
            },
            {targets: "date-type", type: "date-eu"},
            {
                targets: 10,
                data: null,
                defaultContent: "<a class='btn btn-link' role='button' href='#' onclick='edit(this)'>Edit</a>",
                orderable: false,
                searchable: false
            },
        ]
    });
});

function strtrunc(str, num) {
    if (str.length > num) {
        return str.slice(0, num) + "...";
    } else {
        return str;
    }
}

function edit(rowContext) {
    if (table) {
        var data = table.row($(rowContext).parents("tr")).data();
        alert("Example showing row edit with id: " + data["id"] + ", name: " + data["name"]);
    }
}