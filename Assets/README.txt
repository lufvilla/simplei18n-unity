La idea fue aplicar custom editors a una libreria que hice de localization llamada "Simple-i18n".

Para correr el proyecto ir a la carpeta Demo y abrir la escena SampleScene, tiene todas las configuraciones basicas para poder mostrar que la libreria
funciona mas allá de los inspectores.

Tiene una limitación de diseño que es que los archivos que se usan para las configs y traducciones se crean todos dentro de "Resources/Localization",
si esa carpeta se llega a borrar todos los archivos se pueden volver a generar como si fuera la primera vez que correrias la libreria desde el menú de arriba
"Simple localization -> Configure Localization".

Apliqué las consignas del final en las siguientes cosas:

Scriptable Objects:
- General Config
- Languages
- Keys

Custom Windows:
- General Config
- Languages
- Translations

Custom Inspectors:
- Text, TextMeshPro
- Scriptables
- Keys

Gui en escena:
- Aviso de texto mal configurado