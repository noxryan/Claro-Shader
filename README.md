# Claro Shader

Claro Shader is an unofficial tool for the purpose of re-coloring the Dijit theme Claro from the Dojo Toolkit.

## Steps

1. Claro Shader re-colors all of your Claro images

2. .less variables are automatically entered into Claro

3. .less files are compiled into CSS using Node JS

## Requirements

Dojo 1.6.0+ Claro files.

Everything else is included and configured.

## Result

![Claro-Shader](https://github.com/noxryan/Claro-Shader/raw/master/result.png)

## Running With Mono

There are separate Command Line (Claro Shader CLI) and WinForms (Claro Shader) projects included. All processing is handled by Claro Shader Core. The CLI exe will run with Mono.

The CLI version is not pre-packaged with nodeJS or the lessJS module. You will need to have these configured.

CLI Arguments:

**-p**  Path to the Claro directory. (Required)

**-h**  Hue adjustment. (Default: 0)

**-s**  Saturation adjustment. (Default: 0)

**-l**  Luminosity adjustment. (Default: 0)

**-kb** Keep blacks and whites from HSL adjustments. This should generally be used when adjusting luminosity. (Default: true)

**-kg** Keep all shades of gray from HSL adjustments. This should generally be used when adjusting luminosity. (Default: false)

**-gt** Gray tolerance. Setting this above 0 will also ignore off shades of gray. For Claro generally keep this at 0. (Default: 0)

Example: mono "Claro Shader CLI.exe" -p "/dijit/themes/claro" -h 149 -l=-10 -kg true      #This will create a light red Claro theme.

## License

FreeBSD