$(document).ready(function () {
    // Enable Live Search.
    $('#GroupList').attr('data-live-search', true);

    //// Enable multiple select.
    $('#GroupList').attr('multiple', true);
    $('#GroupList').attr('data-selected-text-format', 'count');

    $('.selectGroup').selectpicker(
        {
            width: '100%',
            title: '- [Choose Multiple Countries] -',
            style: 'btn-warning',
            size: 6,
            iconBase: 'fa',
            tickIcon: 'fa-check'
        });
});

