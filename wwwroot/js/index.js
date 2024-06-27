$(document).ready(function(){
  $.ajax({
    type: 'POST',
      url: '~/Controllers/ClienteController/PrimerRequisitoReserva/listaCategoria',
  })
  .done(function(categ){
    $('#categoria').html(categ)
  })
  .fail(function(){
    alert('Hubo un errror al cargar las categoria')
  })

  $('#categoria').on('change', function(){

    $('#doctor').find('option').remove().end().append('<option value="whatever"></option>').val('whatever');

    var id = $('#categoria').val()
    $.ajax({
      type: 'POST',
      url: '../model/MostrarServicio.php',
      data: {'id_categoria': id}
    })
    .done(function(categ){
      $('#servicio').html(categ)
    })
    .fail(function(){
      alert('Hubo un errror al cargar los servicios')
    })
  })

  $('#servicio').on('change', function(){
    var id = $('#servicio').val()
    $.ajax({
      type: 'POST',
      url: '../model/MostrarBarbero.php',
      data: {'id_servicio': id}
    })
    .done(function(categ){
      $('#doctor').html(categ)
    })
    .fail(function(){
      alert('Hubo un errror al cargar los barberos')
    })
  })

  /*$('#enviar').on('click', function(){
    var resultado = 'Lista de reproducción: ' + $('#lista_reproduccion option:selected').text() +
    ' Video elegido: ' + $('#videos option:selected').text()

    $('#resultado1').html(resultado)
  })*/
  /*$.ajax({
    type: 'POST',
    url: '../view/ConsultaDisponibilidad.php',
  })
  .done(function(ca){
    $('#valor').html(ca)
  })
  .fail(function(){
    alert('Hubo un errror al cargar las listas_rep')
  })*/

})