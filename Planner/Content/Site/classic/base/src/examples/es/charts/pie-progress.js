import $ from 'jquery';
import * as Site from 'Site';

$(document).ready(function($) {
  Site.run();
});

// Example Api Methods
// -------------------
(function() {
  var $example = $('#examplePieApi');

  $('.pie-api-start').on('click', function() {
    $example.asPieProgress('start');
  });
  $('.pie-api-finish').on('click', function() {
    $example.asPieProgress('finish');
  });
  $('.pie-api-go').on('click', function() {
    $example.asPieProgress('go', 200);
  });
  $('.pie-api-go_percentage').on('click', function() {
    $example.asPieProgress('go', '50%');
  });
  $('.pie-api-stop').on('click', function() {
    $example.asPieProgress('stop');
  });
  $('.pie-api-reset').on('click', function() {
    $example.asPieProgress('reset');
  });
})();