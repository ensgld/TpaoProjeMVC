$(document).ready(function () {
  //Bunlar kuyulistesini ve wellbore listesini falan göstermek ve gizlemek için
  $(document).on("click", ".toggle-kuyular-btn", function () {
    const target = $(this).data("target");
    $(target).toggleClass("d-none");

    const isVisible = !$(target).hasClass("d-none");
    $(this).text(isVisible ? "Gizle" : "Göster");
  });
  $(document).on("click", ".toggle-wellbore-btn", function () {
    const target = $(this).data("target");
    $(target).toggleClass("d-none");

    const isVisible = !$(target).hasClass("d-none");
    $(this).text(isVisible ? "Gizle" : "Göster");
  });

  //Saha Gridinin Datatable'ı
  $("#sahaTable").DataTable({
    processing: true,
    serverSide: true,
    ajax: {
      url: "Saha/GetSahalarForGrid",
      type: "POST",
    },
    columns: [
      { data: "sahaId" },
      { data: "sahaAdı" },
      { data: "latitude" },
      { data: "longitude" },
      { data: "city" },
      { data: "kuyuCount", class: "text-center" },
      {
        data: "kuyuListHtml",
        orderable: false,
        searchable: false,
      },
      {
        data: "actionButtons",
        orderable: false,
        searchable: false,
        width: "70px",
      },
      { data: "detailsButton", orderable: false, searchable: false },
    ],

    language: {
      url: "https://cdn.datatables.net/plug-ins/1.13.6/i18n/tr.json",
    },
  });

  //Kuyu DataTable
  $("#kuyuTable").DataTable({
    processing: true,
    serverSide: true,
    ajax: {
      url: "Kuyu/GetKuyularForGrid",
      type: "POST",
    },
    columns: [
      { data: "kuyuId" },
      { data: "kuyuAdı" },
      { data: "latitude" },
      { data: "longitude" },
      { data: "city" },
      { data: "wellboreCount", class: "text-center" },
      {
        data: "wellboreListHtml",
        orderable: false,
        searchable: false,
        class: "text-center",
      },
      {
        data: "actionButtons",
        orderable: false,
        searchable: false,
        width: "70px",
      },
      { data: "detailsButton", orderable: false, searchable: false },
    ],

    language: {
      url: "https://cdn.datatables.net/plug-ins/1.13.6/i18n/tr.json",
    },
  });

  //Wellbore DataTable
  $("#wellboreTable").DataTable({
    processing: true,
    serverSide: true,
    ajax: {
      url: "Wellbore/GetWellboresForGrid",
      type: "POST",
    },
    columns: [
      { data: "wellboreId" },
      { data: "wellboreName" },
      { data: "latitude" },
      { data: "longitude" },
      { data: "city" },
      { data: "ownKuyuName" },
      { data: "ownSahaName" },

      {
        data: "actionButtons",
        orderable: false,
        searchable: false,
        width: "70px",
      },
      { data: "detailsButton", orderable: false, searchable: false },
    ],

    language: {
      url: "https://cdn.datatables.net/plug-ins/1.13.6/i18n/tr.json",
    },
  });
});
