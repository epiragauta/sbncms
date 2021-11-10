/// <reference types="Cypress" />
import { PartialViewMacroBuilder } from "sbn-cypress-testhelpers";

context('Partial View Macro Files', () => {

  beforeEach(() => {
    cy.sbnLogin(Cypress.env('username'), Cypress.env('password'));
  });

  function openPartialViewMacroCreatePanel() {
    cy.sbnSection('settings');
    cy.get('li .umb-tree-root:contains("Settings")').should("be.visible");

    cy.sbnTreeItem("settings", ["Partial View Macro Files"]).rightclick();
    cy.sbnContextMenuAction("action-create").click();
  }

  function cleanup(name, extension = ".cshtml") {
    const fileName = name + extension;

    cy.sbnEnsureMacroNameNotExists(name);
    cy.sbnEnsurePartialViewMacroFileNameNotExists(fileName);
  }

  it('Create new partial view macro', () => {
    const name = "TestPartialViewMacro";

    cleanup(name);

    openPartialViewMacroCreatePanel();

    cy.get('.menu-label').first().click(); // TODO: Fucked we cant use something like cy.sbnContextMenuAction("action-label").click();

    //Type name
    cy.sbnEditorHeaderName(name);

    //Save
    cy.get('.btn-success').click();

    //Assert
    cy.sbnSuccessNotification().should('be.visible');
    cy.sbnMacroExists(name).then(exists => { expect(exists).to.be.true; });

    //Clean up
    cleanup(name);
  });

  it('Create new partial view macro without macro', () => {
    const name = "TestPartialMacrolessMacro";

    cleanup(name);

    openPartialViewMacroCreatePanel();

    cy.get('.menu-label').eq(1).click();

    // Type name
    cy.sbnEditorHeaderName(name);

    // Save
    cy.get('.btn-success').click();

    // Assert
    cy.sbnSuccessNotification().should('be.visible');
    cy.sbnMacroExists(name).then(exists => { expect(exists).to.be.false; });

    // Clean
    cleanup(name);
  });

  it('Create new partial view macro from snippet', () => {
    const name = "TestPartialFromSnippet";

    cleanup(name);

    openPartialViewMacroCreatePanel();

    cy.get('.menu-label').eq(2).click();

    // Select snippet
    cy.get('.menu-label').eq(1).click();

    // Type name
    cy.sbnEditorHeaderName(name);

    // Save
    cy.get('.btn-success').click();

    // Assert
    cy.sbnSuccessNotification().should('be.visible');
    cy.sbnMacroExists(name).then(exists => { expect(exists).to.be.true; });

    // Clean
    cleanup(name);
  });

  it('Delete partial view macro', () => {
    const name = "TestDeletePartialViewMacro";
    const fullName = name + ".cshtml"

    cleanup(name);

    const partialViewMacro = new PartialViewMacroBuilder()
      .withName(name)
      .withContent("@inherits Sbn.Web.Macros.PartialViewMacroPage")
      .build();

    cy.savePartialViewMacro(partialViewMacro);

    // Navigate to settings
    cy.sbnSection('settings');
    cy.get('li .umb-tree-root:contains("Settings")').should("be.visible");

    // Delete partialViewMacro
    cy.sbnTreeItem("settings", ["Partial View Macro Files", fullName]).rightclick();
    cy.sbnContextMenuAction("action-delete").click();
    cy.sbnButtonByLabelKey("general_ok").click();

    // Assert
    cy.contains(fullName).should('not.exist');

    // Clean
    cleanup(name);
  });

  it('Edit partial view macro', () => {
    const name = "TestPartialViewMacroEditable";
    const fullName = name + ".cshtml";

    cleanup(name);

    const partialViewMacro = new PartialViewMacroBuilder()
      .withName(name)
      .withContent("@inherits Sbn.Web.Macros.PartialViewMacroPage")
      .build();

    cy.savePartialViewMacro(partialViewMacro);

    // Navigate to settings
    cy.sbnSection('settings');
    cy.get('li .umb-tree-root:contains("Settings")').should("be.visible");
    cy.sbnTreeItem("settings", ["Partial View Macro Files", fullName]).click();

    // Type an edit
    cy.get('.ace_text-input').type(" // test", {force:true} );
    // Save
    cy.get('.btn-success').click();

    // Assert
    cy.sbnSuccessNotification().should('be.visible');

    cleanup(name);
  });

});
