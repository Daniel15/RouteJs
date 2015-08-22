/// <binding BeforeBuild='build' />
var gulp = require('gulp'),
	rename = require('gulp-rename'),
	replace = require('gulp-replace')
	uglify = require('gulp-uglify');

var project = require('./project.json');

gulp.task('build', function () {
	return gulp.src('router.js')
		.pipe(replace('{VERSION}', project.version))
		.pipe(gulp.dest('compiler/resources'))
		.pipe(rename({ extname: '.min.js' }))
		.pipe(uglify({ preserveComments: 'some' }))
		.pipe(gulp.dest('compiler/resources'));

});
