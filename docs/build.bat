rem SET PDF=pdf

SET tmpfile=doc.md

for %%f in (src\*.md) do type "%%f" >> %tmpfile% && echo. >> %tmpfile%

if defined PDF (
    pandoc --listings -H dist/listings-setup.tex -o dist\doc.pdf %tmpfile% --latex-engine=xelatex -V mainfont="Calibri" --toc --toc-depth=4
) else (
    pandoc -o dist\doc.html %tmpfile% -V mainfont="Calibri" --toc --toc-depth=4 -c github-pandoc.css
)

del %tmpfile%

rem pause