﻿/// <binding AfterBuild='default' />
var gulp = require('gulp'),
    gp_concat = require('gulp-concat'),
    gp_rename = require('gulp-rename'),
    gp_uglify = require('gulp-uglify');
var bower = require('gulp-bower');
var karma = require('gulp-karma');
var cssMinify = require('gulp-minify-css');
var cssConcat = require('gulp-concat-css');
var replace = require('gulp-replace');


gulp.task('bower', function () {
    return bower('./bower_components');
});


var cssSources = [
  //'!bower_components/**/*.min.css'
  //
  'bower_components/ngToast/dist/*.min.css',
  'bower_components/angular-material/angular-material.min.css',
  , 'bower_components/bootstrap/dist/css/bootstrap.min.css'
];

var userScriptSources = [
    'Scripts/app/**/*.js'
];

gulp.task('css', function () {

    return gulp.src(cssSources)
    //NOTE: need to clean up scripts before we can do this
     // .pipe(uglify()).on('error', function(err) { console.log(err); })
      .pipe(cssConcat('bro.min.css'))
      .pipe(replace('../bootstrap/dist/fonts/', '../fonts/'))
      .pipe(cssMinify())
      .pipe(gulp.dest('Content'));
});

gulp.task('dependencies', function () {
    return gulp.src(
        [
        'bower_components/jquery/dist/jquery.js',
        'bower_components/jquery-ui/jquery-ui.js',
        'bower_components/**/*.js',
        '!bower_components/**/*min.js', '!bower_components/**/*index.js', '!bower_components/**/*src'])
        .pipe(gp_concat('dependencies.js'))
        .pipe(gp_uglify())
        .pipe(gulp.dest('Content'));
});

gulp.task('broscripts', function () {
    return gulp.src(
        userScriptSources)
        .pipe(gp_concat('broscripts.js'))
        //.pipe(gp_uglify())
        .pipe(gulp.dest('Content'));
});


gulp.task('default', ['bower','dependencies', 'broscripts', 'css'], function () { });