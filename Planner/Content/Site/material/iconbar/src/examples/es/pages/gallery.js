import $ from 'jquery';
import * as Site from 'Site';

$(document).ready(function($) {
  Site.run();

  $('.md-search').magnificPopup({
    type: 'image',
    closeOnContentClick: true,
    mainClass: 'mfp-fade',
    gallery: {
      enabled: true,
      navigateByImgClick: true,
      preload: [0, 1] // Will preload 0 - before current, and 1 after the current image
    }
  });
});
