var gulp = require('gulp'),
    gp_concat = require('gulp-concat'),
    gp_rename = require('gulp-rename'),
    gp_uglify = require('gulp-uglify');

gulp.task('js', function(){
    return gulp.src(
        [
        'bower_components/jquery/dist/jquery.js', 
        'bower_components/jquery-ui/jquery-ui.js', 
        'bower_components/**/*.js', 
        '!bower_components/**/*min.js', '!bower_components/**/*index.js', '!bower_components/**/*src'])
        .pipe(gp_concat('depencies.js'))
        .pipe(gp_uglify())
        .pipe(gulp.dest('src/LiftBro.Web/Content'));
       // .pipe(gp_rename('uglify.js'))
       // .pipe(gp_uglify())
        //.pipe(gulp.dest('test/go'));
});

gulp.task('default', ['js'], function(){});